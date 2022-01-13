using System.Globalization;

namespace DTLib.Dtsod;

// v23
// в процессе создания v30 появились идеи по улучшению 20-ой серии
// новый парсер (опять)
// улучшена сериализация и десериализация листов

public class DtsodV23 : DtsodDict<string, dynamic>, IDtsod
{

    public DtsodVersion Version { get; } = DtsodVersion.V30;
    public IDictionary<string, dynamic> ToDictionary() => this;

    public DtsodV23() : base() => UpdateLazy();
    public DtsodV23(IDictionary<string, dynamic> dict) : base(dict) => UpdateLazy();
    public DtsodV23(string serialized) : this() => Append(Deserialize(serialized));

    static DtsodV23 Deserialize(string text)
    {
        char c;
        int i = -1; // ++i в ReadName
        StringBuilder b = new();
        Dictionary<string, dynamic> output = new();
        bool partOfDollarList = false;
        for (; i < text.Length; i++)
        {
            string name = ReadName();
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
        return new DtsodV23(output);

        string ReadName()
        {
            while (i < text.Length - 1)
            {
                c = text[++i];
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
                        throw new Exception($"DtsodV23.Deserialize() error: unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            throw new Exception("DtsodV23.Deserialize.ReadName() error: end of text\ntext:\n" + text);
        }

        dynamic ReadValue(out bool endOfList)
        {
            endOfList = false;

            void ReadString()
            {
                while ((c != '"' && c != '\'') || (text[i - 1] == '\\' && text[i - 2] != '\\'))
                {
                    b.Append(c);
                    if (++i >= text.Length) throw new Exception("DtsodV23.Deserialize() error: end of text\ntext:\n" + text);
                    c = text[i];
                }
                b.Append('"');
            }

            DtsodV23 ReadDtsod()
            {
                short bracketBalance = 1;
                c = text[++i];    //пропускает начальный символ '{'
                while (bracketBalance != 0)
                {
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
                        case '\'':
                            ReadString();
                            break;
                        default:
                            b.Append(c);
                            break;
                    }

                    if (++i >= text.Length) throw new Exception("DtsodV23.Deserialize() error: end of text\ntext:\n" + text);
                    c = text[i];
                }

                b.Clear();
                return Deserialize(b.ToString());
            }

            List<dynamic> ReadList()
            {
                List<dynamic> list = new();
                while (true)
                {
                    list.Add(ReadValue(out bool _eol));
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
                            return value_str.Substring(1, value_str.Length - 2);
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
                                    return value_str.Contains('.')
                                        ? value_str.ToDouble()
                                        : value_str.ToInt();
                            }
                };
            }

            while (i < text.Length - 1)
            {
                c = text[++i];
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
                    case '\'':
                        ReadString();
                        break;
                    case ';':
                    case ',':
                        string str = b.ToString();
                        b.Clear();
                        return ParseValue(str);
                    case '[':
                        return ReadList();
                    case ']':
                        endOfList = true;
                        break;
                    case '{':
                        return ReadDtsod();
                    case '=':
                    case ':':
                    case '}':
                    case '$':
                        throw new Exception($"DtsodV23.Deserialize() error: unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            throw new Exception("DtsodV23.Deserialize.ReadValue() error: end of text\ntext:\n" + text);
        }

        void SkipComment()
        {
            while (text[i] != '\n')
                if (++i >= text.Length) throw new Exception("DtsodV23.Deserialize() error: end of text\ntext:\n" + text);
        }
    }

    internal static readonly Dictionary<Type, Action<dynamic, StringBuilder>> TypeSerializeFuncs = new()
    {
        { typeof(bool), (val, b) => b.Append(val.ToString()) },
        { typeof(char), (val, b) => b.Append('\'').Append(val).Append('\'') },
        { typeof(string), (val, b) => b.Append('"').Append(val).Append('"') },
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
    short tabscount = -1;
    protected StringBuilder Serialize(IDictionary<string, dynamic> dtsod, StringBuilder b = null)
    {
        tabscount++;
        if (b is null) b = new StringBuilder();
        foreach (var pair in dtsod)
        {
            b.Append('\t', tabscount).Append(pair.Key).Append(": ");
            SerializeType(pair.Value);
            b.Append(";\n");

            void SerializeType(dynamic value)
            {
                if (value is IList _list)
                {
                    b.Append('[');
                    foreach (object el in _list)
                    {
                        SerializeType(el);
                        b.Append(',');
                    }
                    b.Remove(b.Length - 1, 1).Append(']');
                }
                else if (value is IDictionary _dict)
                {
                    b.Append('{');
                    Serialize(value, b);
                    b.Append('}');
                }
                else b.Append(TypeSerializeFuncs[value.GetType()].Invoke(value, b));
            }
        }
        tabscount--;
        return b;
    }

    protected Lazy<string> serialized;
    protected void UpdateLazy() => serialized = new(() => Serialize(this).ToString());
    public override string ToString() => serialized.Value;
}
