using System.Globalization;
using DTLib.Dtsod.ClassSerializer;

namespace DTLib.Dtsod;

public class DtsodV30 : DtsodDict<string, dynamic>, IDtsod
{
    public DtsodVersion Version { get; } = DtsodVersion.V30;

    public IDictionary<string, dynamic> ToDictionary() => this;

    public DtsodV30() : base() => UpdateLazy();
    public DtsodV30(IDictionary<string, dynamic> dict) : base(dict) => UpdateLazy();
    public DtsodV30(string serialized) : this() => Append(Deserialize(serialized));

    static IDictionary<string, dynamic> Deserialize(string text)
    {
        char c;
        int i = -1; // ++i в ReadType
        StringBuilder b = new();


        Type ReadType()
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
                        string _type = b.ToString();
                        b.Clear();
                        return TypeHelper.Instance.TypeFromString(_type);
                    case '=':
                    case '"':
                    case '\'':
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
                    case '=':
                        string _name = b.ToString();
                        b.Clear();
                        return _name;
                    case ':':
                    case '"':
                    case '\'':
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

        object ReadValue(ref bool endoflist)
        {
            void ReadString()
            {
                c = text[++i];    //пропускает начальный символ '"'
                while ((c != '"' && c != '\'') || (text[i - 1] == '\\' && text[i - 2] != '\\'))
                {
                    b.Append(c);
                    if (++i >= text.Length) throw new Exception("DtsodV30.Deserialize() error: end of text\ntext:\n" + text);
                    c = text[i];
                }
            }

            List<object> ReadList()
            {
                List<object> list = new();
                bool _endoflist = false;
                while (!_endoflist)
                    list.Add(CreateInstance(ReadType(), ReadValue(ref _endoflist)));
                b.Clear();
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
                        case '\'':
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

                b.Clear();
                return Deserialize(b.ToString());
            }

            while (i < text.Length-1)
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
                    case ';': // один параметр
                    case ',': // для листов
                        string str = b.ToString();
                        b.Clear();
                        return str == "null" ? null : str;
                    case '[':
                        return ReadList();
                    case ']':
                        endoflist = true;
                        goto case ',';
                    case '{':
                        return ReadDictionary();
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

        object CreateInstance(Type type, object ctor_arg)
        {
            if (TypeHelper.Instance.BaseTypeConstructors.TryGetValue(type, out Func<string, dynamic> ctor))
                return (object)ctor.Invoke((string)ctor_arg);
            else if (type.CustomAttributes.Any(a => a.AttributeType == typeof(DtsodSerializableAttribute)))
                return Activator.CreateInstance(type, ((IDictionary<string, object>)ctor_arg).Values.ToArray());
            else if (typeof(ICollection).IsAssignableFrom(type))
            {
                var method_As = typeof(TypeHelper).GetMethod("As", 
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.NonPublic)
                    .MakeGenericMethod(type.GetGenericArguments()[0]);
                object collection = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                var method_Add = type.GetMethod("Add");
                Log(method_Add.Name);
                foreach (object el in (IEnumerable)ctor_arg)
                {
                    var pel = method_As.Invoke(null, new object[] { el });
                    method_Add.Invoke(collection, new object[] { pel });
                }
                return collection;
            }
            else throw new Exception($"can't create instance of {type.FullName}");
        }

        Dictionary<string, dynamic> output = new();
        Type type;
        string name;
        object value;

        for (; i < text.Length; i++)
        {
            type = ReadType();
            name = ReadName();
            bool _ = false;
            value = ReadValue(ref _);
            output.Add(name, CreateInstance(type, value));
        }

        return output;
    }

    // public override void Append(ICollection<KeyValuePair<string, dynamic>> anotherDtsod) => base.Append(anotherDtsod);//UpdateLazy();

    public override void Add(string key, dynamic value) => base.Add(key, (object)value);//UpdateLazy();

    protected static string Serialize(IDictionary<string, dynamic> dtsod, ushort tabsCount = 0)
    {
        StringBuilder b = new();
        foreach (KeyValuePair<string, dynamic> pair in dtsod)
        {
            SerializeObject(pair.Key, pair.Value);
        }

        void SerializeObject(string name, dynamic inst)
        {
            Type type = inst.GetType();
            b.Append(TypeHelper.Instance.TypeToString(type)).Append(':')
                .Append(name).Append('=');
            if (TypeHelper.Instance.BaseTypeNames.ContainsKey(type))
            {
                if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
                    b.Append(inst.ToString(CultureInfo.InvariantCulture));
                else b.Append(inst.ToString());
            }
            else if (typeof(IDictionary<string, dynamic>).IsAssignableFrom(type))
                b.Append("\n{\n").Append(Serialize(inst, tabsCount++)).Append("};\n");
            else if (type.CustomAttributes.Any(a => a.AttributeType == typeof(DtsodSerializableAttribute)))
            {
                var props = type.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(DtsodSerializableAttribute)));
                foreach (var prop in props)
                {
                    var propval = prop.GetValue(inst);

                }
            }
            else throw new Exception($"can't serialize type {type.FullName}");
        }

        return b.ToString();
    }

    protected Lazy<string> serialized;
    protected void UpdateLazy() => serialized = new(() => Serialize(this));
    public override string ToString() => serialized.Value;
}
