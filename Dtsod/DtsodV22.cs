using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTLib.Dtsod
{
    //
    // это как json но не совсем
    //
    // v20
    // полностью переписан парсер
    //  
    // v21
    // парсер теперь не может игнорировать комменты, потом починю
    // теперь числовые значения конвертируются в правильный тип, а не в int64/uint64 (новый вариант switch из c#9.0 делал какую-то херню)
    // исправлены некоторые другие баги
    // 
    // v22
    // метод ToString() теперь действительно деконструирует объект в текст, а не возвращает параметр text из конструктора

    public class DtsodV22 : Dictionary<string, DtsodV22.ValueStruct>
    {
        static readonly bool debug = false;

        public struct ValueStruct
        {
            public dynamic Value;
            public ValueTypes Type;
            public bool IsList;
            public ValueStruct(ValueTypes type, dynamic value, bool isList)
            {
                Value=value;
                Type=type;
                IsList=isList;
            }
            public ValueStruct(ValueTypes type, dynamic value)
            {
                Value=value;
                Type=type;
                IsList=false;
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
            foreach(KeyValuePair<string, ValueStruct> pair in Parse(text))
                Add(pair.Key, pair.Value);
        }

        public DtsodV22(Dictionary<string, DtsodV22.ValueStruct> dict)
        {
            foreach(KeyValuePair<string, ValueStruct> pair in dict)
                Add(pair.Key, pair.Value);
        }

        // выдаёт Exception
        public new dynamic this[string key]
        {
            get
            {
                if(TryGetValue(key, out dynamic value))
                    return value;
                else
                    throw new Exception($"Dtsod[{key}] key not found");
            }
            set
            {
                if(TrySetValue(key, value))
                    return;
                else
                    throw new Exception($"Dtsod[{key}] key not found");
            }
        }

        // не выдаёт KeyNotFoundException
        public bool TryGetValue(string key, out dynamic value)
        {
            try
            {
                value=base[key].Value;
                return true;
            }
            catch(KeyNotFoundException)
            {
                value=null;
                return false;
            }
        }
        public bool TrySetValue(string key, dynamic value)
        {
            try
            {
                bool isList;
                if(value is IList)
                    isList=true;
                else
                    isList=false;
                base[key]=new(base[key].Type, value, isList);
                return true;
            }
            catch(KeyNotFoundException)
            {
                return false;
            }
        }

        DtsodV22 Parse(string text)
        {
            Dictionary<string, ValueStruct> parsed = new();
            int i = 0;
            for(; i<text.Length; i++)
                ReadName();
            DebugNoTime("g", $"Parse returns {parsed.Keys.Count} keys\n");
            return new DtsodV22(parsed);

            // СЛОМАНО
            /*void ReadCommentLine()
            {
                for (; i < text.Length && text[i] != '\n'; i++) Debug("gray", text[i].ToString());
            }*/

            void ReadName()
            {

                bool isListElem = false;
                dynamic value = null;
                StringBuilder defaultNameBuilder = new();

                DebugNoTime("m", "ReadName\n");
                for(; i<text.Length; i++)
                {
                    switch(text[i])
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case ':':
                            i++;
                            string name = defaultNameBuilder.ToString();
                            value=ReadValue(out ValueTypes type, out bool isList);
                            DebugNoTime("c", $"parsed.Add({name},{type} {value} )\n");
                            if(isListElem)
                            {
                                if(!parsed.ContainsKey(name))
                                    parsed.Add(name, new(type, new List<dynamic>(), isList));
                                parsed[name].Value.Add(value);
                            }
                            else
                                parsed.Add(name, new(type, value, isList));
                            return;
                        // строка, начинающаяся с # будет считаться комментом
                        case '#':
                            //ReadCommentLine();
                            break;
                        case '}':
                            throw new Exception("Parse.ReadName() error: unexpected '}' at "+i+" char");
                        // если $ перед названием параметра поставить, значение value добавится в лист с названием name
                        case '$':
                            DebugNoTime("w", text[i].ToString());
                            if(defaultNameBuilder.ToString().Length!=0)
                                throw new Exception("Parse.ReadName() error: unexpected '$' at "+i+" char");
                            isListElem=true;
                            break;
                        case ';':
                            throw new Exception("Parse.ReadName() error: unexpected ';' at "+i+" char");
                        default:
                            DebugNoTime("w", text[i].ToString());
                            defaultNameBuilder.Append(text[i]);
                            break;
                    }
                }
            }

            dynamic ReadValue(out ValueTypes outType, out bool isList)
            {
                ValueTypes type = ValueTypes.Unknown;
                isList=false;
                dynamic value = null;

                string ReadString()
                {
                    i++;
                    StringBuilder valueBuilder = new();
                    valueBuilder.Append('"');
                    for(; text[i]!='"'||text[i-1]=='\\'; i++)
                    {
                        DebugNoTime("gray", text[i].ToString());
                        valueBuilder.Append(text[i]);
                    }
                    valueBuilder.Append('"');
                    DebugNoTime("gray", text[i].ToString());
                    type=ValueTypes.String;
                    return valueBuilder.ToString();
                }

                List<dynamic> ReadList()
                {
                    i++;
                    List<dynamic> output = new();
                    StringBuilder valueBuilder = new();
                    for(; text[i]!=']'; i++)
                    {
                        DebugNoTime("c", text[i].ToString());
                        switch(text[i])
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
                    if(valueBuilder.Length>0)
                    {
                        ParseValueToRightType(valueBuilder.ToString());
                        output.Add(value);
                    }
                    DebugNoTime("c", text[i].ToString());
                    type=ValueTypes.List;
                    return output;
                }

                Dictionary<string, ValueStruct> ReadComplex()
                {
                    StringBuilder valueBuilder = new();
                    int balance = 1;
                    i++;
                    for(; balance!=0; i++)
                    {
                        DebugNoTime("y", text[i].ToString());
                        switch(text[i])
                        {
                            case '"':
                                valueBuilder.Append(ReadString());
                                break;
                            case '}':
                                balance--;
                                DebugNoTime("b", $"\nbalance -- = {balance}\n");
                                if(balance!=0)
                                    valueBuilder.Append(text[i]);
                                break;
                            case '{':
                                balance++;
                                DebugNoTime("b", $"\nbalance ++ = {balance}\n");
                                valueBuilder.Append(text[i]);
                                break;
                            default:
                                valueBuilder.Append(text[i]);
                                break;
                        }
                    }
                    i--;   // i++ в for выполняется даже когда balance == 0, то есть text[i] получается == ;, что ломает всё
                    type=ValueTypes.Complex;
                    return Parse(valueBuilder.ToString());
                }

                void ParseValueToRightType(string stringValue)
                {
                    DebugNoTime("b", $"\nParseValueToRightType({stringValue})\n");
                    switch(stringValue)
                    {

                        // bool
                        case "true":
                        case "false":
                            type=ValueTypes.Bool;
                            value=stringValue.ToBool();
                            break;
                        // null
                        case "null":
                            type=ValueTypes.Null;
                            value=null;
                            break;
                        default:
                            if(stringValue.Contains('"'))
                            {
                                type=ValueTypes.String;
                                value=stringValue.Remove(stringValue.Length-1).Remove(0, 1);
                            }
                            // double
                            else if(stringValue.Contains('.'))
                            {
                                type=ValueTypes.Double;
                                value=stringValue.ToDouble();
                            }
                            // ushort; ulong; uint
                            else if(stringValue.Length>2&&stringValue[stringValue.Length-2]=='u')
                            {
                                switch(stringValue[stringValue.Length-1])
                                {
                                    case 's':
                                        type=ValueTypes.UShort;
                                        value=stringValue.Remove(stringValue.Length-2).ToUShort();
                                        break;
                                    case 'i':
                                        type=ValueTypes.UInt;
                                        value=stringValue.Remove(stringValue.Length-2).ToUInt();
                                        break;
                                    case 'l':
                                        type=ValueTypes.ULong;
                                        value=stringValue.Remove(stringValue.Length-2).ToULong();
                                        break;
                                    default:
                                        throw new Exception($"Dtsod.Parse.ReadValue() error: value <{stringValue}> has wrong type");
                                };
                            }
                            // short; long; int
                            else
                                switch(stringValue[stringValue.Length-1])
                                {
                                    case 's':
                                        type=ValueTypes.Short;
                                        value=stringValue.Remove(stringValue.Length-1).ToShort();
                                        break;
                                    case 'l':
                                        type=ValueTypes.Long;
                                        value=stringValue.Remove(stringValue.Length-1).ToLong();
                                        break;
                                    default:
                                        type=ValueTypes.Int;
                                        value=stringValue.ToShort();
                                        break;
                                }
                            break;
                    };
                }

                StringBuilder defaultValueBuilder = new();
                DebugNoTime("m", "\nReadValue\n");
                for(; i<text.Length; i++)
                {
                    DebugNoTime("b", text[i].ToString());
                    switch(text[i])
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case '"':
                            value=ReadString();
                            break;
                        case '[':
                            value=ReadList();
                            break;
                        case '{':
                            value=ReadComplex();
                            break;
                        case ';':
                            switch(type)
                            {
                                case ValueTypes.String:
                                    ParseValueToRightType(value);
                                    break;
                                case ValueTypes.Unknown:
                                    ParseValueToRightType(defaultValueBuilder.ToString());
                                    break;
                                case ValueTypes.List:
                                    isList=true;
                                    break;
                            };
                            outType=type;
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
            foreach(string key in dtsod.Keys)
            {
                outBuilder.Append('\t', tabCount);
                outBuilder.Append(key);
                outBuilder.Append(": ");
                dtsod.TryGetValue(key, out ValueStruct value);
                switch(value.Type)
                {
                    case ValueTypes.List:
                        outBuilder.Append("\"list deconstruction is'nt implemented yet\"");
                        break;
                    case ValueTypes.Complex:
                        outBuilder.Append("\n");
                        outBuilder.Append('\t', tabCount);
                        outBuilder.Append("{\n");
                        tabCount++;
                        outBuilder.Append(Deconstruct(value.Value));
                        tabCount--;
                        outBuilder.Append('\t', tabCount);
                        outBuilder.Append("}");
                        break;
                    case ValueTypes.String:
                        outBuilder.Append("\"");
                        outBuilder.Append(value.Value.ToString());
                        outBuilder.Append("\"");
                        break;
                    case ValueTypes.Short:
                        outBuilder.Append(value.Value.ToString());
                        outBuilder.Append("s");
                        break;
                    case ValueTypes.Int:
                        outBuilder.Append(value.Value.ToString());
                        break;
                    case ValueTypes.Long:
                        outBuilder.Append(value.Value.ToString());
                        outBuilder.Append("l");
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

        void DebugNoTime(params string[] msg)
        {
            if(debug)
                PublicLog.LogNoTime(msg);
        }

        public DtsodV22 Extend(DtsodV22 newPart)
        {
            foreach(KeyValuePair<string, ValueStruct> pair in newPart)
                Add(pair.Key, pair.Value);
            return this;
        }

        public void Add(KeyValuePair<string, ValueStruct> a) => Add(a.Key, a.Value);
    }
}
