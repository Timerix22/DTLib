#include "DtsodV24.h"
#include "../Autoarr/StringBuilder.h"

#define ARR_BC 2
#define ARR_BL 8

Hashtable* __deserialize(char* text, bool calledRecursively){
    Hashtable* dict=Hashtable_create();
    char c;
    bool partOfDollarList=false;
    bool readingList=false;

    void throw_wrongchar(char _c){
                char errBuf[]="unexpected <c> at:\n  \""
                    "00000000000000000000000000000000"
                    "\"";
                errBuf[12]=_c;
                for(uint8 i=0;i<32;i++)
                    errBuf[i+22]=*(text-16+i);
                throw(errBuf);
    };

    void SkipComment(){
        while((c=*++text)!='\n')
            if(!c) throw(ERR_ENDOFSTR);
    };

    string ReadName(){
        string nameStr={text,0};
        while ((c=*++text)) switch (c){
            case ' ':  case '\t':
            case '\r': case '\n':
                break;
            case '#':
                SkipComment();
                break;
            case '}':
                if(!calledRecursively) throw_wrongchar(c);
            case ':':
                return nameStr;
            case '$':
                partOfDollarList=true;
                break;
            case '=':  case ';':
            case '\'': case '"':
            case '[':  case ']':
            case '{':
                throw_wrongchar(c);
                break;
            default:
                nameStr.length++;
                break;
        }

        if(nameStr.length>0) throw(ERR_ENDOFSTR);
        return nameStr;
    };

    Unitype ReadValue(){
        string valueStr={text,0};

        //returns part of <text> with quotes
        string ReadString(){
            bool prevIsBackslash = false;
            string str={text,1};
            while ((c=*++text)!='"' || prevIsBackslash){
                printf("%c",c);
                if (!c) throw(ERR_ENDOFSTR);
                prevIsBackslash= c=='\\' && !prevIsBackslash;
                str.length++;
            }
            str.length++;
            return str;
        };

        Autoarr2(Unitype)* ReadList(){
            Autoarr2(Unitype)* list=malloc(sizeof(Autoarr2(Unitype)));
            *list=Autoarr2_create(Unitype,ARR_BC,ARR_BL);
            readingList=true;
            while (true){
                Autoarr2_add(list,ReadValue());
                if (!readingList) break;
            }
            return list;
        };

        Hashtable* ReadDtsod(){
            if(*++text) //skips {
                return __deserialize(text,true);
            else {
                throw(ERR_ENDOFSTR);
                return NULL;
            }
        };

        Unitype ParseValue(string str){
            const string nullStr={"null",4};
            const string trueStr={"true",4};
            const string falseStr={"false",5};
            switch(*str.ptr){
                case 'n':
                    if(string_compare(str,nullStr))
                       return UniNull;
                    else throw_wrongchar(*str.ptr);
                    break;
                case 't':
                    if(string_compare(str,trueStr))
                       return UniTrue;
                    else throw_wrongchar(*str.ptr);
                    break;
                case 'f':
                    if(string_compare(str,falseStr))
                       return UniFalse;
                    else throw_wrongchar(*str.ptr);
                    break;
                case '"':
                    if(str.ptr[str.length-1]=='"'){
                        //removing quotes
                        string _str={str.ptr+1,str.length-1};
                        return UniPtr(CharPtr, string_cpToCharPtr(_str));
                    }
                    else throw_wrongchar(*str.ptr);
                    break;
                default: 
                    switch(str.ptr[str.length-1]){
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
                                /* if (value_str.Contains('.'))
                                    return (object)(value_str.ToDouble());
                                else
                                {
                                    try { return (object)(value_str.ToInt()); }
                                    catch (FormatException)
                                    {
                                        Log("r", $"can't parse value: {value_str}");
                                        return null;
                                    }
                                } */
                    }
            }
            throw(ERR_NOTIMPLEMENTED);
            return UniNull;
        };

        while ((c=*++text)) switch (c){
            case ' ':  case '\t':
            case '\r': case '\n':
                break;
            case '#':
                SkipComment();
                break;
            case '"':
                valueStr=ReadString();
                break;
            case '\'':
                text++;
                char valueChar=*++text;
                if (*++text != '\'') throw("after <'> should be char");
                else if (valueStr.length!=0) throw("char assignement error");
                else return Uni(Char,valueChar);
                break;
            case ';':
            case ',':
                return ParseValue(valueStr);
            case '[':
                return UniPtr(AutoarrUnitypePtr,ReadList());
            case ']':
                readingList=false;
                break;
            case '{':
                return UniPtr(HashtablePtr,ReadDtsod());
            case '=': case ':': 
            case '}': case '$':
                throw_wrongchar(c);
                break;
            default:
                valueStr.length++;
                break;
        }

        throw(ERR_ENDOFSTR);
        return UniNull;
    };

    while((c=*text++)){
        string name=ReadName();
        if(name.length==0) //end of file or '}' in recursive call 
            return dict;
        char* nameCPtr=string_cpToCharPtr(name);
        Unitype value=ReadValue();
        if(partOfDollarList){
            Autoarr2(Unitype)* list;
            Unitype lu;
            if(Hashtable_try_get(dict,nameCPtr, &lu))
                list=(Autoarr2(Unitype)*)lu.VoidPtr;
            else{
                list=malloc(sizeof(Autoarr2(Unitype)));
                *list=Autoarr2_create(Unitype,ARR_BC,ARR_BL);
                Hashtable_add(dict,nameCPtr,UniPtr(AutoarrUnitypePtr,list));
            }
            Autoarr2_add(list,value);
        }
        else Hashtable_add(dict,nameCPtr,value);
    }
    return dict;
}

Hashtable* DtsodV24_deserialize(char* text) { return __deserialize(text,false); }
