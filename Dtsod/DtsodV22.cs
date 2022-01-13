namespace DTLib.Dtsod;

// v22
// метод ToString() теперь деконструирует объект в текст, а не возвращает параметр text из конструктора
// деконструкция листов не работает из-за костыльного определения типов данных

public class DtsodV22 : Dictionary<string, DtsodV22.ValueStruct>, IDtsod
{
    public DtsodVersion Version { get; } = DtsodVersion.V22;

    public IDictionary<string, dynamic> ToDictionary()
    {
        Dictionary<string, dynamic> newdict = new();
        foreach (KeyValuePair<string, ValueStruct> pair in this)
            newdict.Add(pair.Key, pair.Value.Value);
        return newdict;
    }

    public struct ValueStruct
    {
        public dynamic Value;
        public ValueTypes Type;
        public bool IsList;
        public ValueStruct(ValueTypes type, dynamic value, bool isList)
        {
            Value = value;
            Type = type;
            IsList = isList;
        }
    }

    public enum ValueTypes
    {
        List,
        Complex,
        String,
        Short,
        Int,
        Long,
        UShort,
        UInt,
        ULong,
        Double,
        Null,
        Bool,
        Unknown
    }

    public DtsodV22() { }

    public DtsodV22(string text)
    {
        foreach (KeyValuePair<string, ValueStruct> pair in Parse(text))
            Add(pair.Key, pair.Value);
    }

    public DtsodV22(Dictionary<string, ValueStruct> dict)
    {
        foreach (KeyValuePair<string, ValueStruct> pair in dict)
            Add(pair.Key, pair.Value);
    }

    // выдаёт Exception
    public new dynamic this[string key]
    {
        get => TryGetValue(key, out dynamic value) ? value : throw new Exception($"Dtsod[{key}] key not found");
        set
        {
            if (!TrySetValue(key, value)) throw new Exception($"Dtsod[{key}] key not found");
        }
    }

    // не выдаёт KeyNotFoundException
    public bool TryGetValue(string key, out dynamic value)
    {
        try
        {
            value = base[key].Value;
            return true;
        }
        catch (KeyNotFoundException)
        {
            value = null;
            return false;
        }
    }
    public bool TrySetValue(string key, dynamic value)
    {
        try
        {
            bool isList = value is IList;
            base[key] = new(base[key].Type, value, isList);
            return true;
        }
        catch (KeyNotFoundException)
        { return false; }
    }

    DtsodV22 Parse(string text)
    {
        Dictionary<string, ValueStruct> parsed = new();
        int i = 0;
        for (; i < text.Length; i++)
            ReadName();
#if DEBUG
        DebugNoTime("g", $"Parse returns {parsed.Keys.Count} keys\n");
#endif
        return new DtsodV22(parsed);

        // СЛОМАНО
        /*void ReadCommentLine()
        {
            for (; i < text.Length && text[i] != '\n'; i++) Debug("h", text[i].ToString());
        }*/

        void ReadName()
        {

            bool isListElem = false;
            dynamic value;
            StringBuilder defaultNameBuilder = new();

#if DEBUG
            DebugNoTime("m", "ReadName\n");
#endif
            for (; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        break;
                    case ':':
                        i++;
                        string name = defaultNameBuilder.ToString();
                        value = ReadValue(out ValueTypes type, out bool isList);
#if DEBUG
                        DebugNoTime("c", $"parsed.Add({name},{type} {value} )\n");
#endif
                        if (isListElem)
                        {
                            if (!parsed.ContainsKey(name))
                                parsed.Add(name, new(type, new List<dynamic>(), isList));
                            parsed[name].Value.Add(value);
                        }
                        else parsed.Add(name, new(type, value, isList));
                        return;
                    // строка, начинающаяся с # будет считаться комментом
                    case '#':
                        //ReadCommentLine();
                        break;
                    case '}':
                        throw new Exception("Parse.ReadName() error: unexpected '}' at " + i + " char");
                    // если $ перед названием параметра поставить, значение value добавится в лист с названием name
                    case '$':
#if DEBUG
                        DebugNoTime("w", text[i].ToString());
#endif
                        if (defaultNameBuilder.ToString().Length != 0)
                            throw new Exception("Parse.ReadName() error: unexpected '$' at " + i + " char");
                        isListElem = true;
                        break;
                    case ';':
                        throw new Exception("Parse.ReadName() error: unexpected ';' at " + i + " char");
                    default:
#if DEBUG
                        DebugNoTime("w", text[i].ToString());
#endif
                        defaultNameBuilder.Append(text[i]);
                        break;
                }
            }
        }

        dynamic ReadValue(out ValueTypes outType, out bool isList)
        {
            ValueTypes type = ValueTypes.Unknown;
            isList = false;
            dynamic value = null;

            string ReadString()
            {
                i++;
                StringBuilder valueBuilder = new();
                valueBuilder.Append('"');
                for (; text[i] != '"' || text[i - 1] == '\\'; i++)
                {
#if DEBUG
                    DebugNoTime("h", text[i].ToString());
#endif
                    valueBuilder.Append(text[i]);
                }
                valueBuilder.Append('"');
#if DEBUG
                DebugNoTime("h", text[i].ToString());
#endif
                type = ValueTypes.String;
                return valueBuilder.ToString();
            }

            List<dynamic> ReadList()
            {
                i++;
                List<dynamic> output = new();
                StringBuilder valueBuilder = new();
                for (; text[i] != ']'; i++)
                {
#if DEBUG
                    DebugNoTime("c", text[i].ToString());
#endif
                    switch (text[i])
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case ',':
                            ParseValueToRightType(valueBuilder.ToString());
                            output.Add(value);
                            valueBuilder.Clear();
                            break;
                        default:
                            valueBuilder.Append(text[i]);
                            break;
                    }
                }
                if (valueBuilder.Length > 0)
                {
                    ParseValueToRightType(valueBuilder.ToString());
                    output.Add(value);
                }
#if DEBUG
                DebugNoTime("c", text[i].ToString());
#endif
                type = ValueTypes.List;
                return output;
            }

            Dictionary<string, ValueStruct> ReadComplex()
            {
                StringBuilder valueBuilder = new();
                int balance = 1;
                i++;
                for (; balance != 0; i++)
                {
#if DEBUG
                    DebugNoTime("y", text[i].ToString());
#endif
                    switch (text[i])
                    {
                        case '"':
                            valueBuilder.Append(ReadString());
                            break;
                        case '}':
                            balance--;
#if DEBUG
                            DebugNoTime("b", $"\nbalance -- = {balance}\n");
#endif
                            if (balance != 0)
                                valueBuilder.Append(text[i]);
                            break;
                        case '{':
                            balance++;
#if DEBUG
                            DebugNoTime("b", $"\nbalance ++ = {balance}\n");
#endif
                            valueBuilder.Append(text[i]);
                            break;
                        default:
                            valueBuilder.Append(text[i]);
                            break;
                    }
                }
                i--;   // i++ в for выполняется даже когда balance == 0, то есть text[i] получается == ;, что ломает всё
                type = ValueTypes.Complex;
                return Parse(valueBuilder.ToString());
            }

            void ParseValueToRightType(string stringValue)
            {
#if DEBUG
                DebugNoTime("b", $"\nParseValueToRightType({stringValue})\n");
#endif
                switch (stringValue)
                {

                    // bool
                    case "true":
                    case "false":
                        type = ValueTypes.Bool;
                        value = stringValue.ToBool();
                        break;
                    // null
                    case "null":
                        type = ValueTypes.Null;
                        value = null;
                        break;
                    default:
                        if (stringValue.Contains('"'))
                        {
                            type = ValueTypes.String;
                            value = stringValue.Remove(stringValue.Length - 1).Remove(0, 1);
                        }
                        // double
                        else if (stringValue.Contains('.'))
                        {
                            type = ValueTypes.Double;
                            value = stringValue.ToDouble();
                        }
                        // ushort; ulong; uint
                        else if (stringValue.Length > 2 && stringValue[stringValue.Length - 2] == 'u')
                        {
                            switch (stringValue[stringValue.Length - 1])
                            {
                                case 's':
                                    type = ValueTypes.UShort;
                                    value = stringValue.Remove(stringValue.Length - 2).ToUShort();
                                    break;
                                case 'i':
                                    type = ValueTypes.UInt;
                                    value = stringValue.Remove(stringValue.Length - 2).ToUInt();
                                    break;
                                case 'l':
                                    type = ValueTypes.ULong;
                                    value = stringValue.Remove(stringValue.Length - 2).ToULong();
                                    break;
                                default:
                                    throw new Exception($"Dtsod.Parse.ReadValue() error: value <{stringValue}> has wrong type");
                            };
                        }
                        // short; long; int
                        else
                            switch (stringValue[stringValue.Length - 1])
                            {
                                case 's':
                                    type = ValueTypes.Short;
                                    value = stringValue.Remove(stringValue.Length - 1).ToShort();
                                    break;
                                case 'l':
                                    type = ValueTypes.Long;
                                    value = stringValue.Remove(stringValue.Length - 1).ToLong();
                                    break;
                                default:
                                    type = ValueTypes.Int;
                                    value = stringValue.ToInt();
                                    break;
                            }
                        break;
                };
            }

            StringBuilder defaultValueBuilder = new();
#if DEBUG
            DebugNoTime("m", "\nReadValue\n");
#endif
            for (; i < text.Length; i++)
            {
#if DEBUG
                DebugNoTime("b", text[i].ToString());
#endif
                switch (text[i])
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        break;
                    case '"':
                        value = ReadString();
                        break;
                    case '[':
                        value = ReadList();
                        break;
                    case '{':
                        value = ReadComplex();
                        break;
                    case ';':
                        switch (type)
                        {
                            case ValueTypes.String:
                                ParseValueToRightType(value);
                                break;
                            case ValueTypes.Unknown:
                                ParseValueToRightType(defaultValueBuilder.ToString());
                                break;
                            case ValueTypes.List:
                                isList = true;
                                break;
                        };
                        outType = type;
                        return value;
                    // строка, начинающаяся с # будет считаться комментом
                    case '#':
                        //ReadCommentLine();
                        break;
                    default:
                        defaultValueBuilder.Append(text[i]);
                        break;
                }
            }
            throw new Exception("Dtsod.Parse.ReadValue error: wtf it's the end of function");
        }
    }

    public override string ToString() => Deconstruct(this);

    ushort tabCount = 0;
    string Deconstruct(DtsodV22 dtsod)
    {
        StringBuilder outBuilder = new();
        foreach (string key in dtsod.Keys)
        {
            outBuilder.Append('\t', tabCount);
            outBuilder.Append(key);
            outBuilder.Append(": ");
            dtsod.TryGetValue(key, out ValueStruct value);
            switch (value.Type)
            {
                case ValueTypes.List:
                    outBuilder.Append('[').Append(StringConverter.MergeToString((IEnumerable<object>)value.Value, ",")).Append(']');
                    //outBuilder.Append("\"list deconstruction is'nt implemented yet\"");
                    break;
                case ValueTypes.Complex:
                    outBuilder.Append('\n');
                    outBuilder.Append('\t', tabCount);
                    outBuilder.Append("{\n");
                    tabCount++;
                    outBuilder.Append(Deconstruct(value.Value));
                    tabCount--;
                    outBuilder.Append('\t', tabCount);
                    outBuilder.Append('}');
                    break;
                case ValueTypes.String:
                    outBuilder.Append('\"');
                    outBuilder.Append(value.Value.ToString());
                    outBuilder.Append('\"');
                    break;
                case ValueTypes.Short:
                    outBuilder.Append(value.Value.ToString());
                    outBuilder.Append('s');
                    break;
                case ValueTypes.Int:
                    outBuilder.Append(value.Value.ToString());
                    break;
                case ValueTypes.Long:
                    outBuilder.Append(value.Value.ToString());
                    outBuilder.Append('l');
                    break;
                case ValueTypes.UShort:
                    outBuilder.Append(value.Value.ToString());
                    outBuilder.Append("us");
                    break;
                case ValueTypes.UInt:
                    outBuilder.Append(value.Value.ToString());
                    outBuilder.Append("ui");
                    break;
                case ValueTypes.ULong:
                    outBuilder.Append(value.Value.ToString());
                    outBuilder.Append("ul");
                    break;
                case ValueTypes.Double:
                    outBuilder.Append(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    break;
                case ValueTypes.Null:
                    outBuilder.Append("null");
                    break;
                case ValueTypes.Bool:
                    outBuilder.Append(value.Value.ToString().ToLower());
                    break;
                default:
                    throw new Exception($"Dtsod.Deconstruct() error: unknown type: {value.Type}");
            }

            outBuilder.Append(";\n");
        }
        return outBuilder.ToString();
    }

    public void Add(KeyValuePair<string, ValueStruct> a) => Add(a.Key, a.Value);

    public DtsodV22 Extend(DtsodV22 newPart)
    {
        foreach (KeyValuePair<string, ValueStruct> pair in newPart)
            Add(pair.Key, pair.Value);
        return this;
    }

#if DEBUG
    static void Debug(params string[] msg) => PublicLog.Log(msg);
    static void DebugNoTime(params string[] msg) => PublicLog.LogNoTime(msg);
#endif
}
