#include "DtsodV24.h"

#define ARR_BC 8
#define ARR_BL 16

Hashtable* __deserialize(char** _text, bool calledRecursively){
    Hashtable* dict=Hashtable_create();
    char* text=*_text;
    char c;
    bool partOfDollarList=false;
    bool readingList=false;

    void __throw_wrongchar(char* file, int line, char* fname,char _c){
                char errBuf[]="unexpected <c> at:\n  \""
                    "00000000000000000000000000000000"
                    "\"";
                errBuf[12]=_c;
                for(uint8 i=0;i<32;i++)
                    errBuf[i+22]=*(text-16+i);
                printf("\n\e[31m[%s:%d/%s]\n",file,line,fname);
                throw(errBuf);
    };
    #define throw_wrongchar(C) __throw_wrongchar(__FILE__,__LINE__,__func__,C)
    

    void SkipComment(){
        while((c=*++text)!='\n')
            if(!c) throw(ERR_ENDOFSTR);
    };

    string ReadName(){
        string nameStr={text,0};
        text--;
        while ((c=*++text)) switch (c){
            case ' ':  case '\t':
            case '\r': case '\n':
                if(nameStr.length!=0)
                    throw_wrongchar(c);
                nameStr.ptr++;
                break;
            case '=':  case ';':
            case '\'': case '"':
            case '[':  case ']':
            case '{':
                throw_wrongchar(c);
                break;
            case '#':
                SkipComment();
                if(nameStr.length!=0)
                    throw_wrongchar(c);
                nameStr.ptr=text;
                break;
            case '}':
                if(!calledRecursively) throw_wrongchar(c);
                if((*++text)!=';')
                    throw_wrongchar(c);
            case '$':
                if(nameStr.length!=0)
                    throw_wrongchar(c);
                nameStr.ptr++;
                partOfDollarList=true;
                break;
            case ':':
                return nameStr;
            default:
                nameStr.length++;
                break;
        }

        if(nameStr.length>0) throw(ERR_ENDOFSTR);
        return nameStr;
    };

    Unitype ReadValue(){

        //returns part of <text> without quotes
        string ReadString(){
            bool prevIsBackslash=false;
            string str={text+1,0};
            while ((c=*++text)!='"' || prevIsBackslash){
                if (!c) throw(ERR_ENDOFSTR);
                prevIsBackslash= c=='\\' && !prevIsBackslash;
                str.length++;
            }
            return str;
        };

        Autoarr(Unitype)* ReadList(){
            Autoarr(Unitype)* list=malloc(sizeof(Autoarr(Unitype)));
            *list=Autoarr_create(Unitype,ARR_BC,ARR_BL);
            readingList=true;
            while (true){
                Autoarr_add(list,ReadValue());
                if (!readingList) break;
            }
            return list;
        };

        Hashtable* ReadDtsod(){
            if(*++text) //skips {
                return __deserialize(&text,true);
            else {
                throw(ERR_ENDOFSTR);
                return NULL;
            }
        };

        Unitype ParseValue(string str){
            printf("\e[94m<\e[96m%s\e[94m>\n",string_cpToCharPtr(str));
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
                default: 
                    switch(str.ptr[str.length-1]){
                        case 'f': {
                                char* _c=string_cpToCharPtr(str);
                                Unitype rez=Uni(Double,strtod(_c,NULL));
                                free(_c);
                                return rez;
                            }
                        case 'u': {
                                uint64 lu=0;
                                char* _c=string_cpToCharPtr(str);
                                sscanf(_c,"%lu",&lu);
                                free(_c);
                                return Uni(UInt64,lu);
                            }
                        case '0': case '1': case '2': case '3': case '4':
                        case '5': case '6': case '7': case '8': case '9': {
                                int64 li=0;
                                char* _c=string_cpToCharPtr(str);
                                if(sscanf(_c,"%li",&li)!=1){
                                    char err[64];
                                    sprintf(err,"can't parse to int: <%s>",_c);
                                    throw(err);
                                }
                                    
                                free(_c);
                                return Uni(Int64,li);
                            }
                        default:
                            throw_wrongchar(str.ptr[str.length-1]);
                    }
            }
            throw(ERR_ENDOFSTR);
            return UniNull;
        };

        string valueStr={text+1,0};
        Unitype value;
        while ((c=*++text)) switch (c){
            case ' ':  case '\t':
            case '\r': case '\n':
                if(valueStr.length!=0)
                    throw_wrongchar(c);
                valueStr.ptr++;
                break;
            case '=': case ':': 
            case '}': case '$':
                throw_wrongchar(c);
                break;
            case '#':
                SkipComment();
                if(valueStr.length!=0)
                    throw_wrongchar(c);
                valueStr.ptr=text;
                break;
            case '"':
                if(valueStr.length!=0) throw_wrongchar(c);
                value=UniPtr(CharPtr,string_cpToCharPtr(ReadString()));
                break;
            case '\'':
                if(valueStr.length!=0) throw_wrongchar(c);
                text++;
                char valueChar=*++text;
                if (*++text != '\'') throw("after <'> should be char");
                value=Uni(Char,valueChar);
                break;
            case '[':
                if(valueStr.length!=0) throw_wrongchar(c);
                value=UniPtr(AutoarrUnitypePtr,ReadList());
            case ']':
                readingList=false;
                break;
            case '{':
                if(valueStr.length!=0) throw_wrongchar(c);
                value=UniPtr(HashtablePtr,ReadDtsod());
            case ';':
            case ',':
                if(valueStr.length!=0)
                    value=ParseValue(valueStr);
                return value;
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
        if(name.length==0) //end of file or '}' in recursive call 
            goto END;
        char* nameCPtr=string_cpToCharPtr(name);
        Unitype value=ReadValue();
        if(partOfDollarList){
            Autoarr(Unitype)* list;
            Unitype lu;
            if(Hashtable_try_get(dict,nameCPtr, &lu)){
                list=(Autoarr(Unitype)*)lu.VoidPtr;
            }
            else{
                list=malloc(sizeof(Autoarr(Unitype)));
                *list=Autoarr_create(Unitype,ARR_BC,ARR_BL);
                Hashtable_add(dict,nameCPtr,UniPtr(AutoarrUnitypePtr,list));
            }
            Autoarr_add(list,value);
        }
        else Hashtable_add(dict,nameCPtr,value);
    }
    END:
    *_text=text;
    return dict;
}

Hashtable* DtsodV24_deserialize(char* text) {
    Hashtable* r=__deserialize(&text,false); 
    return r;
}
