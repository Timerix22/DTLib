using System.Globalization;
using System.Linq;

namespace DTLib.Dtsod;

public class DtsodV30 : DtsodDict<string, dynamic>, IDtsod
{
    public DtsodVersion Version { get; } = DtsodVersion.V30;

    public IDictionary<string, dynamic> ToDictionary() => this;

    public DtsodV30() : base() => UpdateLazy();
    public DtsodV30(IDictionary<string, dynamic> dict) : base(dict) => UpdateLazy();
    public DtsodV30(string serialized) : this() => Append(Deserialize(serialized));

#if DEBUG
    static void DebugLog(params string[] msg) => PublicLog.Log(msg);
#endif

    static IDictionary<string, dynamic> Deserialize(string text)
    {
        char c;
        int i = -1; // ++i в ReadType
        StringBuilder b = new();


        Type ReadType()
        {

            while (i < text.Length)
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
                        string _type = b.ToString();
                        b.Clear();
                        return TypeHelper.TypeFromString(_type);
                    case '=':
                    case '"':
                    case ';':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                        throw new Exception($"DtsodV30.Deserialize() error: unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            throw new Exception("DtsodV30.Deserialize.ReadType() error: end of text\ntext:\n" + text);
        }

        string ReadName()
        {
            while (i < text.Length)
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
                    case '=':
                        string _name = b.ToString();
                        b.Clear();
                        return _name;
                    case ':':
                    case '"':
                    case ';':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                        throw new Exception($"DtsodV30.Deserialize() error: unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            throw new Exception("DtsodV30.Deserialize.ReadName() error: end of text\ntext:\n" + text);
        }

        object[] ReadValue()
        {
            void ReadString()
            {
                c = text[++i];    //пропускает начальный символ '"'
                while (c != '"' || (text[i - 1] == '\\' && text[i - 2] != '\\'))
                {
                    b.Append(c);
                    if (++i >= text.Length) throw new Exception("DtsodV30.Deserialize() error: end of text\ntext:\n" + text);
                    c = text[i];
                }
            }

            bool endoflist = false;  // выставляется в цикле в ReadValue()
            List<object> ReadList()
            {
                List<dynamic> list = new();
                while (!endoflist)
                    list.Add(CreateInstance(ReadType(), ReadValue()));
                endoflist = false;
                return list;
            }

            IDictionary<string, dynamic> ReadDictionary()
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
                            b.Append('"');
                            ReadString();
                            b.Append('"');
                            break;
                        default:
                            b.Append(c);
                            break;
                    }

                    if (++i >= text.Length) throw new Exception("DtsodV30.Deserialize() error: end of text\ntext:\n" + text);
                    c = text[i];
                }

                return Deserialize(b.ToString());
            }

            while (i < text.Length)
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
                        ReadString();
                        break;
                    case ';': // один параметр
                    case ',': // для листов
                        string str = b.ToString();
                        b.Clear();
                        // hardcoded "null" value
                        return str == "null" ? (new object[] { null }) : (new object[] { str });
                    case '[':
                        {
                            object[] _value = ReadList().ToArray();
                            b.Clear();
                            return _value;
                        }
                    case ']':
                        endoflist = true;
                        goto case ',';
                    case '{':
                        {
                            object[] _value = new object[] { ReadDictionary() };
                            b.Clear();
                            return _value;
                        }
                    case '=':
                    case ':':
                    case '}':
                        throw new Exception($"DtsodV30.Deserialize() error: unexpected {c}");
                    default:
                        b.Append(c);
                        break;
                }
            }

            throw new Exception("DtsodV30.Deserialize.ReadValue() error: end of text\ntext:\n" + text);
        }

        void SkipComment()
        {
            while (text[i] != '\n')
                if (++i >= text.Length) throw new Exception("DtsodV30.Deserialize() error: end of text\ntext:\n" + text);
        }

        object CreateInstance(Type type, object[] ctor_args)
        {
            if (TypeHelper.BaseTypeConstructors.TryGetValue(type, out Func<string, dynamic> ctor))
                return (object)ctor.Invoke((string)ctor_args[0]);
            else if (type.CustomAttributes.Any(a => a.AttributeType == typeof(DtsodSerializableAttribute)))
                return Activator.CreateInstance(type, ctor_args);
            else throw new Exception($"type {type.AssemblyQualifiedName} doesn't have DtsodSerializableAttribute");
        }

        Dictionary<string, dynamic> output = new();
        Type type;
        string name;
        object[] value;

        for (; i < text.Length; i++)
        {
            type = ReadType();
            name = ReadName();
            value = ReadValue();
            output.Add(name, CreateInstance(type, value));
        }

        return output;
    }

    public override void Append(ICollection<KeyValuePair<string, dynamic>> anotherDtsod) => base.Append(anotherDtsod);//UpdateLazy();

    public override void Add(string key, dynamic value) => base.Add(key, (object)value);//UpdateLazy();

    protected static string Serialize(IDictionary<string, dynamic> dtsod, ushort tabsCount = 0)
    {
        StringBuilder b = new();
        foreach (KeyValuePair<string, dynamic> pair in dtsod)
        {
            Type type = pair.Value.GetType();
            b.Append(TypeHelper.TypeToString(type)).Append(':')
                .Append(pair.Key).Append('=');
            if (TypeHelper.BaseTypeNames.ContainsKey(type))
            {
                if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
                    b.Append(pair.Value.ToString(CultureInfo.InvariantCulture));
                else b.Append(pair.Value.ToString());
            }
            else if (typeof(IDictionary<string, dynamic>).IsAssignableFrom(type))
                b.Append("\n{\n").Append(Serialize(pair.Value, tabsCount++)).Append("};\n");
            else
            {
                type.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(DtsodSerializableAttribute)));

            }
        }

        return b.ToString();
    }

    protected Lazy<string> serialized;
    protected void UpdateLazy() => serialized = new(() => Serialize(this));
    public override string ToString() => serialized.Value;
}
