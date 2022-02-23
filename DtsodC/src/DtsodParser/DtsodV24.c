#include "DtsodV24.h"
#include "../Autoarr/StringBuilder.h"

#define ARR_BC 2
#define ARR_BL 8

Hashtable* __parse(const char* text, bool called_recursively){
    Hashtable* dict=Hashtable_create();
    char c;
    bool partOfDollarList=false;

    void SkipComment(){
        while((c=*text++)!='\n')
            if(!c) throw(ERR_ENDOFSTR);
    };

    char* ReadName(){
        StringBuilder b=StringBuilder_create(4,32);
        while ((c=*text)){
            switch (c){
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    break;
                case '#':
                    SkipComment();
                    break;
                case ':':
                    return StringBuilder_build(&b);
                case '$':
                    partOfDollarList=true;
                    break;
                case '=':  case ';':
                case '\'': case '"':
                case '[':  case ']':
                case '{':  case '}':
                    char errstr[]="unexpected <c>";
                    errstr[12]=c;
                    throw(errstr);
                    break;
                default:
                    StringBuilder_append(&b,c);
                    break;
            }
            text++;
        }
        if(Autoarr2_length((&b))>0) throw(ERR_ENDOFSTR);
        return NULL;
    };

    Unitype ReadValue(){
        Unitype value=UniNull;

        return value;
    };


    while((c=*text++)){
        char* name=ReadName();
        if(!name){
            free(name);
            return dict;
        }
        Unitype value=ReadValue();
        if(partOfDollarList){
            Autoarr2(Unitype)* list;
            Unitype _lu;
            if(Hashtable_try_get(dict,name, &_lu))
                list=(Autoarr2(Unitype)*)_lu.VoidPtr;
            else {
                list=malloc(sizeof(Autoarr2(Unitype)));
                *list=Autoarr2_create(Unitype,ARR_BC,ARR_BL);
                Hashtable_add(dict,name,UniF(AutoarrUnitypePtr,VoidPtr,list));
            }
            Autoarr2_add(list,value);
        }
        else Hashtable_add(dict,name,value);
    }
    
    return dict;
}

Hashtable* DtsodV24_parse(const char* text) { return __parse(text,false); }
