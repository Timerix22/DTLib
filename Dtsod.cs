using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DTLib.PublicLog;

namespace DTLib
{
    //
    // это как json но не совсем
    //
    public class Dtsod : Dictionary<string, dynamic>
    {
        static readonly bool debug = false;

        public string Text { get; }
        //public Dictionary<string, dynamic> Values { get; set; }
        public Dtsod(string text)
        {
            Text = text;
            foreach (KeyValuePair<string, dynamic> pair in Parse(text))
                Add(pair.Key, pair.Value);
        }

        // выдаёт Exception
        new public dynamic this[string key]
        {
            get
            {
                if (TryGetValue(key, out dynamic value)) return value;
                else throw new Exception($"Dtsod[{key}] key not found");
            }
            set
            {
                if (TrySetValue(key, value)) return;
                else throw new Exception($"Dtsod[{key}] key not found");
            }
        }

        // не выдаёт KeyNotFoundException
        new public bool TryGetValue(string key, out dynamic value)
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
            /*Double,
            Long,
            Ulong,
            Short,
            Ushort,
            Int,
            Uint,
            Null,
            Boolean,*/
            Default
        }

        Dictionary<string, dynamic> Parse(string text)
        {
            Dictionary<string, dynamic> parsed = new();
            int i = 0;
            for (; i < text.Length; i++) ReadName();
            return parsed;

            void ReadName()
            {
                void ReadCommentLine()
                {
                    for (; i < text.Length && text[i] != '\n'; i++) ;
                }

                bool isListElem = false;
                dynamic value = null;
                StringBuilder defaultNameBuilder = new();

                if (debug) LogNoTime("m", "ReadName");
                for (; i < text.Length; i++)
                {
                    if (debug) LogNoTime("w", text[i].ToString());
                    switch (text[i])
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case ':':
                            i++;
                            value = ReadValue();
                            string name = defaultNameBuilder.ToString();
                            if (debug) LogNoTime("c", $"parsed.Add({name}, {value})\n");
                            if (isListElem)
                            {
                                if (!parsed.ContainsKey(name)) parsed.Add(name, new List<dynamic>());
                                parsed[name].Add(value);
                            }
                            else parsed.Add(name, value);
                            if (debug) LogNoTime("g", "ReadName return\n");
                            return;
                        // строка, начинающаяся с # будет считаться комментом
                        case '#':
                            ReadCommentLine();
                            break;
                        case '}':
                            throw new Exception("Parse.ReadName() error: unexpected '}' at " + i + " char");
                        // если $ перед названием параметра поставить, значение value добавится в лист с названием name
                        case '$':
                            if (defaultNameBuilder.ToString().Length != 0) throw new Exception("Parse.ReadName() error: unexpected '$' at " + i + " char");
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

                string ReadString()
                {
                    i++;
                    StringBuilder valueBuilder = new();
                    valueBuilder.Append('"');
                    for (; text[i] != '"' || text[i - 1] == '\\'; i++)
                    {
                        if (debug) LogNoTime("gray", text[i].ToString());
                        valueBuilder.Append(text[i]);
                    }
                    valueBuilder.Append('"');
                    if (debug) LogNoTime("gray", text[i].ToString());
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
                        if (debug) LogNoTime("c", text[i].ToString());
                        switch (text[i])
                        {
                            case ' ':
                            case '\t':
                            case '\r':
                            case '\n':
                                break;
                            case ',':
                                output.Add(ParseValueToRightType(valueBuilder.ToString()));
                                valueBuilder.Clear();
                                break;
                            default:
                                valueBuilder.Append(text[i]);
                                break;
                        }
                    }
                    if (valueBuilder.Length > 0)
                        output.Add(ParseValueToRightType(valueBuilder.ToString()));
                    if (debug) LogNoTime("c", text[i].ToString());
                    type = ValueType.List;
                    return output;
                }

                Dictionary<string, dynamic> ReadComplex()
                {
                    i++;
                    StringBuilder valueBuilder = new();
                    for (; text[i] != '}'; i++)
                    {
                        if (debug) LogNoTime("y", text[i].ToString());
                        if (text[i] == '"')
                        {
                            valueBuilder.Append(ReadString());
                        }
                        else valueBuilder.Append(text[i]);
                    }
                    if (debug) LogNoTime("y", text[i].ToString());
                    type = ValueType.Complex;
                    if (debug) LogNoTime("g", valueBuilder.ToString());
                    return Parse(valueBuilder.ToString());
                }

                dynamic ParseValueToRightType(string stringValue)
                {

                    if (debug) LogNoTime("g", $"\nParseValueToRightType({stringValue})");
                    return stringValue switch
                    {
                        _ when stringValue.Contains('"') => stringValue.Remove(stringValue.Length - 1).Remove(0, 1),
                        // bool
                        "true" or "false" => stringValue.ToBool(),
                        // null
                        "null" => null,
                        // double
                        _ when stringValue.Contains('.') => stringValue.ToDouble(),
                        // ushort, ulong, uint
                        _ when (stringValue.Length > 2 && stringValue[stringValue.Length - 2] == 'u') => stringValue[stringValue.Length - 1] switch
                        {
                            's' => stringValue.Remove(stringValue.Length - 2).ToUShort(),
                            'i' => stringValue.Remove(stringValue.Length - 2).ToUInt(),
                            'l' => stringValue.Remove(stringValue.Length - 2).ToULong(),
                            _ => throw new Exception($"Dtsod.Parse.ReadValue() error: wrong type <u{stringValue[stringValue.Length - 1]}>")
                        },
                        // short, long, int
                        _ => stringValue[stringValue.Length - 1] switch
                        {
                            's' => stringValue.Remove(stringValue.Length - 1).ToShort(),
                            'l' => stringValue.Remove(stringValue.Length - 1).ToLong(),
                            _ => stringValue.ToInt()
                        }
                    };
                }

                dynamic value = null;
                StringBuilder defaultValueBuilder = new();
                if (debug) LogNoTime("m", "\nReadValue\n");
                for (; i < text.Length; i++)
                {
                    if (debug) LogNoTime("b", text[i].ToString());
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
                            if (debug) LogNoTime("g", $"\nReadValue returns type {type} value <{value}>\n");
                            return type switch
                            {
                                ValueType.List or ValueType.Complex => value,
                                ValueType.String => ParseValueToRightType(value),
                                ValueType.Default => ParseValueToRightType(defaultValueBuilder.ToString()),
                                _ => throw new Exception($"Dtlib.Parse.ReadValue() error: can't convert value to type <{type}>")
                            };
                        case '[':
                            value = ReadList();
                            break;
                        case '{':
                            value = ReadComplex();
                            break;
                        default:
                            defaultValueBuilder.Append(text[i]);
                            break;
                    }
                }
                throw new Exception("Dtsod.Parse.ReadValue error: end of text");
            }
        }
    }
}
