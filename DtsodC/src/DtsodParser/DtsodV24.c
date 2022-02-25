#include "DtsodV24.h"
#include "../Autoarr/StringBuilder.h"

#define ARR_BC 2
#define ARR_BL 8

Hashtable* __parse(const char* text, bool called_recursively){
    Hashtable* dict=Hashtable_create();
    bool partOfDollarList=false;
    char c;

    void throw_wrongchar(char _c){
                char errstr[]="unexpected <c>";
                errstr[12]=_c;
                throw(errstr);
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
                if(!called_recursively) throw_wrongchar(c);
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
            default:
                nameStr.length++;
                break;
        }

        if(nameStr.length>0) throw(ERR_ENDOFSTR);
        return nameStr;
    };

    Unitype ReadValue(){
        string valueStr={text,0};
        bool endOfList=false;
        string ReadString(){
            bool prevIsBackslash = false;
            string str={text,1};
            while ((c=*++text)!='"' || prevIsBackslash){
                if (!c) throw(ERR_ENDOFSTR);
                prevIsBackslash= c=='\\' && !prevIsBackslash;
                str.length++;
            }
            str.length++;
            return str;
        };
        Autoarr2(Unitype)* ReadList(){

            throw(ERR_ENDOFSTR);
            return NULL;
        };
        Hashtable* ReadDtsod(){

            throw(ERR_ENDOFSTR);
            return NULL;
        };
        Unitype ParseValue(string str){

            throw(ERR_ENDOFSTR);
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
                endOfList=true;
                break;
            case '{':
                return UniPtr(HashtablePtr,ReadDtsod());
            case '=': case ':': 
            case '}': case '$':
                throw_wrongchar(c);
            default:
                valueStr.length++;
                break;
        }

        throw(ERR_ENDOFSTR);
        return UniNull;
    };

    text--;
    while((c=*++text)){
        string name=ReadName();
        if(name.length==0) //closing figure bracket in recursive call or end of file
            return dict;
        char* nameCPtr=string_toCharPtr(name);
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

Hashtable* DtsodV24_parse(const char* text) { return __parse(text,false); }
