namespace DTLib.Dtsod;

// v21
// парсер теперь не может игнорировать комменты, потом починю
// теперь числовые значения конвертируются в правильный тип, а не в int64/uint64 (новый вариант switch из c#9.0 делал какую-то дичь)
// исправлены некоторые другие баги

public class DtsodV21 : Dictionary<string, dynamic>, IDtsod
{
    public DtsodVersion Version { get; } = DtsodVersion.V21;

    public IDictionary<string, dynamic> ToDictionary() => this;

    readonly string Text;

    //public Dictionary<string, dynamic> Values { get; set; }
    public DtsodV21(string text)
    {
        Text = text;
        foreach (KeyValuePair<string, dynamic> pair in Parse(text))
            Add(pair.Key, pair.Value);
    }
    public DtsodV21(IDictionary<string, dynamic> rawDict)
    {
        Text = "";
        foreach (KeyValuePair<string, dynamic> pair in rawDict)
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
    public new bool TryGetValue(string key, out dynamic value)
    {
        try
        {
            value = base[key];
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
            base[key] = value;
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public override string ToString() => Text;

    enum ValueType
    {
        List,
        Complex,
        String,
        Default
    }

    Dictionary<string, dynamic> Parse(string text)
    {
        Dictionary<string, dynamic> parsed = new();
        int i = 0;
        for (; i < text.Length; i++)
            ReadName();
        return parsed;

        // СЛОМАНО
        /*void ReadCommentLine()
        {
            for (; i < text.Length && text[i] != '\n'; i++) DebugNoTime("h", text[i].ToString());
        }*/

        void ReadName()
        {

            bool isListElem = false;
            dynamic value;
            StringBuilder defaultNameBuilder = new();

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
                        value = ReadValue();
                        // если value это null, эта строка выдавала ошибку
                        //DebugNoTime("c", $"parsed.Add({name}, {value} { value.GetType() })");
                        if (isListElem)
                        {
                            if (!parsed.ContainsKey(name))
                                parsed.Add(name, new List<dynamic>());
                            parsed[name].Add(value);
                        }
                        else
                            parsed.Add(name, value);
                        return;
                    // строка, начинающаяся с # будет считаться комментом
                    case '#':
                        //ReadCommentLine();
                        break;
                    case '}':
                        throw new Exception("Parse.ReadName() error: unexpected '}' at " + i + " char");
                    // если $ перед названием параметра поставить, значение value добавится в лист с названием name
                    case '$':
                        if (defaultNameBuilder.ToString().Length != 0)
                            throw new Exception("Parse.ReadName() error: unexpected '$' at " + i + " char");
                        isListElem = true;
                        break;
                    case ';':
                        throw new Exception("Parse.ReadName() error: unexpected ';' at " + i + " char");
                    default:
                        defaultNameBuilder.Append(text[i]);
                        break;
                }
            }
        }

        dynamic ReadValue()
        {
            ValueType type = ValueType.Default;
            dynamic value = null;

            string ReadString()
            {
                i++;
                StringBuilder valueBuilder = new();
                valueBuilder.Append('"');
                for (; text[i] != '"' || text[i - 1] == '\\'; i++)
                {
                    valueBuilder.Append(text[i]);
                }
                valueBuilder.Append('"');
                type = ValueType.String;
                return valueBuilder.ToString();
            }

            List<dynamic> ReadList()
            {
                i++;
                List<dynamic> output = new();
                StringBuilder valueBuilder = new();
                for (; text[i] != ']'; i++)
                {
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
                type = ValueType.List;
                return output;
            }

            Dictionary<string, dynamic> ReadComplex()
            {
                StringBuilder valueBuilder = new();
                int balance = 1;
                i++;
                for (; balance != 0; i++)
                {
                    switch (text[i])
                    {
                        case '"':
                            valueBuilder.Append(ReadString());
                            break;
                        case '}':
                            balance--;
                            if (balance != 0)
                                valueBuilder.Append(text[i]);
                            break;
                        case '{':
                            balance++;
                            valueBuilder.Append(text[i]);
                            break;
                        default:
                            valueBuilder.Append(text[i]);
                            break;
                    }
                }
                i--;   // i++ в for выполняется даже когда balance == 0, то есть text[i] получается == ;, что ломает всё
                type = ValueType.Complex;
                return Parse(valueBuilder.ToString());
            }

            void ParseValueToRightType(string stringValue)
            {

                switch (stringValue)
                {

                    // bool
                    case "true":
                    case "false":
                        value = stringValue.ToBool();
                        break;
                    // null
                    case "null":
                        value = null;
                        break;
                    default:
                        if (stringValue.Contains('"'))
                            value = stringValue.Remove(stringValue.Length - 1).Remove(0, 1);
                        // double
                        else if (stringValue.Contains('.'))
                            value = stringValue.ToDouble();
                        // ushort; ulong; uint
                        else if (stringValue.Length > 2 && stringValue[stringValue.Length - 2] == 'u')
                        {
                            switch (stringValue[stringValue.Length - 1])
                            {
                                case 's':
                                    value = stringValue.Remove(stringValue.Length - 2).ToUShort();
                                    break;
                                case 'i':
                                    value = stringValue.Remove(stringValue.Length - 2).ToUInt();
                                    break;
                                case 'l':
                                    value = stringValue.Remove(stringValue.Length - 2).ToULong();
                                    break;
                                default:
                                    throw new Exception($"Dtsod.Parse.ReadValue() error: value= wrong type <u{stringValue[stringValue.Length - 1]}>");
                            }
                        }
                        // short; long; int
                        else
                            switch (stringValue[stringValue.Length - 1])
                            {
                                case 's':
                                    value = stringValue.Remove(stringValue.Length - 1).ToShort();
                                    break;
                                case 'l':
                                    value = stringValue.Remove(stringValue.Length - 1).ToLong();
                                    break;
                                default:
                                    value = stringValue.ToInt();
                                    break;
                            }
                        break;
                }
            }

            StringBuilder defaultValueBuilder = new();
            for (; i < text.Length; i++)
            {
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
                    case ';':
                        switch (type)
                        {
                            case ValueType.String:
                                ParseValueToRightType(value);
                                break;
                            case ValueType.Default:
                                ParseValueToRightType(defaultValueBuilder.ToString());
                                break;
                        }

                        return value;
                    case '[':
                        value = ReadList();
                        break;
                    case '{':
                        value = ReadComplex();
                        break;
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
}

