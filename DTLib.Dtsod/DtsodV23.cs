using System.Globalization;

namespace DTLib.Dtsod;

// v23
// в процессе создания v30 появились идеи по улучшению 20-ой серии
// новый парсер (опять)
// улучшена сериализация и десериализация листов

public class DtsodV23 : DtsodDict<string, dynamic>, IDtsod
{

    public DtsodVersion Version { get; } = DtsodVersion.V23;
    public IDictionary<string, dynamic> ToDictionary() => this;

    public DtsodV23() {}
    public DtsodV23(IDictionary<string, dynamic> dict) : base(dict) {}
    public DtsodV23(string serialized) => this.Append(Deserialize(serialized));

    static DtsodV23 Deserialize(string _text)
    {
        char[] text = _text.ToArray();
        char c;
        int i = -1; // ++i в ReadName
        StringBuilder b = new();
        Dictionary<string, dynamic> output = new();
        bool partOfDollarList = false;
        while (i < text.Length)
        {
            string name = ReadName();
            if (name.IsNullOrEmpty()) goto end;
            dynamic value = ReadValue(out bool _);
            if (partOfDollarList)
            {
                if (!output.TryGetValue(name, out var dollarList))
                {
                    dollarList = new List<dynamic>();
                    output.Add(name, dollarList);
                }
                dollarList.Add(value);
            }
            else output.Add(name, value);
        }
        end:return new DtsodV23(output);

        string ReadName()
        {
            while (++i < text.Length)
            {
                c = text[i];
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        break;
                    case '#':
                        SkipComment();
                        break;
                    case ':':
                        string _name = b.ToString();
                        b.Clear();
                        return _name;
                    case '$':
                        partOfDollarList = true;
                        break;
                    case '=':
                    case '"':
                    case '\'':
                    case ';':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                        throw new Exception($"unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            return b.Length == 0
                ? ""
                : throw new Exception("DtsodV23.Deserialize.ReadName() error: end of text\ntext:\n" + _text);
        }

        dynamic ReadValue(out bool endOfList)
        {
            endOfList = false;

            void ReadString()
            {
                bool prevIsBackslash = false;
                b.Append('"');
                c = text[++i];
                while (c != '"' || prevIsBackslash)
                {
                    prevIsBackslash = c == '\\' && !prevIsBackslash;
                    b.Append(c);
                    if (++i >= text.Length) throw new Exception("end of text\ntext:\n" + _text);
                    c = text[i];
                }
                b.Append('"');
            }

            DtsodV23 ReadDtsod()
            {
                short bracketBalance = 1;
                while (bracketBalance != 0)
                {
                    if (++i >= text.Length) //пропускает начальный символ '{'
                        throw new Exception("end of text\ntext:\n" + _text);
                    c = text[i];
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case '#':
                            SkipComment();
                            break;
                        case '{':
                            bracketBalance++;
                            b.Append(c);
                            break;
                        case '}':
                            bracketBalance--;
                            if (bracketBalance != 0)
                                b.Append(c);
                            break;
                        case '"':
                            ReadString();
                            break;
                        default:
                            b.Append(c);
                            break;
                    }
                }

                var __text = b.ToString();
                b.Clear();
                return Deserialize(__text);
            }

            List<dynamic> ReadList()
            {
                List<dynamic> list = new();
                while (true)
                {
                    var item = ReadValue(out bool _eol);
                    if(item!=null)
                        list.Add(item);
                    if (_eol) break;
                }

                b.Clear();
                return list;
            }

            dynamic ParseValue(string value_str)
            {
                switch (value_str)
                {
                    case "true":
                    case "false":
                        return value_str.ToBool();
                    case "null":
                        return null;
                    default:
                        if (value_str.Contains('"'))
                            return value_str.Substring(1, value_str.Length - 2).Replace("\\\\","\\").Replace("\\\"","\"");
                        else if (value_str.Contains('\''))
                            return value_str[1];
                        else switch (value_str[value_str.Length - 1])
                        {
                            case 's':
                                return value_str[value_str.Length - 2] == 'u'
                                    ? value_str.Remove(value_str.Length - 2).ToUShort()
                                    : value_str.Remove(value_str.Length - 1).ToShort();
                            case 'u':
                                return value_str.Remove(value_str.Length - 1).ToUInt();
                            case 'i':
                                return value_str[value_str.Length - 2] == 'u'
                                    ? value_str.Remove(value_str.Length - 2).ToUInt()
                                    : value_str.Remove(value_str.Length - 1).ToInt();
                            case 'l':
                                return value_str[value_str.Length - 2] == 'u'
                                    ? value_str.Remove(value_str.Length - 2).ToULong()
                                    : value_str.Remove(value_str.Length - 1).ToLong();
                            case 'b':
                                return value_str[value_str.Length - 2] == 's'
                                    ? value_str.Remove(value_str.Length - 2).ToSByte()
                                    : value_str.Remove(value_str.Length - 1).ToByte();
                            case 'f':
                                return value_str.Remove(value_str.Length - 1).ToFloat();
                            case 'e':
                                return value_str[value_str.Length - 2] == 'd'
                                    ? value_str.Remove(value_str.Length - 2).ToDecimal()
                                    : throw new Exception("can't parse value:" + value_str);
                            default:
                                if (value_str.Contains('.'))
                                    return (object)(value_str.ToDouble());
                                else
                                {
                                    try { return (object)(value_str.ToInt()); }
                                    catch (FormatException)
                                    {
                                        Log("r", $"can't parse value: {value_str}");
                                        return null;
                                    }
                                }
                        }
                }
            }

            object value = null;
            while (++i < text.Length)
            {
                c = text[i];
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        break;
                    case '#':
                        SkipComment();
                        break;
                    case '"':
                        ReadString();
                        break;
                    case '\'':
                        b.Append(c).Append(text[++i]);
                        c = text[++i];
                        if (c != '\'') 
                            throw new Exception("after <\'> should be char");
                        b.Append(c);
                        break;
                    case ';':
                    case ',':
                        if(b.Length == 0)
                            return value;
                        string str = b.ToString();
                        b.Clear();
                        return ParseValue(str);
                    case '[':
                        value=ReadList();
                        goto case ';';
                    case ']':
                        endOfList = true;
                        break;
                    case '{':
                        value=ReadDtsod();
                        break;
                    case '=':
                    case ':':
                    case '}':
                    case '$':
                        throw new Exception($"unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            throw new Exception("DtsodV23.Deserialize.ReadValue() error: end of text\ntext:\n" + _text);
        }

        void SkipComment()
        {
            while (i < text.Length && text[i] != '\n') 
                i++;
        }
    }

    internal static readonly Dictionary<Type, Action<dynamic, StringBuilder>> TypeSerializeFuncs = new()
    {
        { typeof(bool), (val, b) => b.Append((bool)val ? "true" : "false") },
        { typeof(char), (val, b) => b.Append('\'').Append(val).Append('\'') },
        { typeof(string), (val, b) => b.Append('"').Append(val.Replace("\\","\\\\").Replace("\"", "\\\"")).Append('"') },
        { typeof(byte), (val, b) => b.Append(val.ToString()).Append('b') },
        { typeof(sbyte), (val, b) => b.Append(val.ToString()).Append("sb") },
        { typeof(short), (val, b) => b.Append(val.ToString()).Append('s') },
        { typeof(ushort), (val, b) => b.Append(val.ToString()).Append("us") },
        { typeof(int), (val, b) => b.Append(val.ToString()) },
        { typeof(uint), (val, b) => b.Append(val.ToString()).Append("ui") },
        { typeof(long), (val, b) => b.Append(val.ToString()).Append('l') },
        { typeof(ulong), (val, b) => b.Append(val.ToString()).Append("ul") },
        { typeof(float), (val, b) => b.Append(val.ToString(CultureInfo.InvariantCulture)).Append('f') },
        { typeof(double), (val, b) => b.Append(val.ToString(CultureInfo.InvariantCulture)) },
        { typeof(decimal), (val, b) => b.Append(val.ToString(CultureInfo.InvariantCulture)).Append("de") }
    };
    
    protected StringBuilder Serialize(DtsodV23 dtsod, ref int tabscount, StringBuilder b = null)
    {
        tabscount++;
        b ??= new StringBuilder();
        foreach (var pair in dtsod)
        {
            b.Append('\t', tabscount).Append(pair.Key).Append(": ");
            SerializeType(pair.Value, ref tabscount);
            b.Append(";\n");

            void SerializeType(dynamic value, ref int tabscount)
            {
                if (value is null) b.Append("null");
                else if (value is DtsodV23 _dtsod)
                {
                    b.Append("{\n");
                    Serialize(_dtsod, ref tabscount, b);
                    b.Append('}');
                }
                else if (value is IList _list)
                {
                    b.Append('[');
                    if(_list.Count>0){
                        foreach (object el in _list)
                        {
                            SerializeType(el, ref tabscount);
                            b.Append(',');
                        }
                        b.Remove(b.Length - 1, 1);
                    }
                    b.Append(']');
                }
                else TypeSerializeFuncs[value.GetType()].Invoke(value, b);
            }
        }
        tabscount--;
        return b;
    }

    public override string ToString()
    {
        int tabscount = -1;
        return Serialize(this, ref tabscount).ToString();
    }

    ///serializes dtsod as part of list
    public string ToString(string partialListName) 
    {
        int tabscount = 0;
        var builder = new StringBuilder().Append('$').Append(partialListName).Append(":\n{\n");
        return Serialize(this, ref tabscount, builder).Append("};\n").ToString();
    }
}
