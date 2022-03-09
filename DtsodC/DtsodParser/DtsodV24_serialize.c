#include "DtsodV24.h"
#include "../Autoarr/StringBuilder.h"

//65536 max length!
#define STRB_BC 64
#define STRB_BL 1024

#define addc(B,C) StringBuilder_append_char(B,C)

void __serialize(StringBuilder* b, uint8 tabs, Hashtable* dtsod){
    
    void AppendTabs(){
        for(uint8 t=0; t<tabs; t++)
            addc(b,'\t');
    };
    
    void AppendValue(Unitype u){
        switch(u.type){
                case Int64:
                    StringBuilder_append_int64(b,u.Int64);
                    break;
                case UInt64:
                    StringBuilder_append_uint64(b,u.UInt64);
                    addc(b,'u');
                    break;
                case Double:
                    StringBuilder_append_double(b,u.Double);
                    addc(b,'f');
                    break;
                case CharPtr:
                    addc(b,'"');
                    char c;
                    while((c=*(char*)(u.VoidPtr++))){
                        if(c=='\"') addc(b,'\\');
                        addc(b,c);
                    }
                    addc(b,'"');
                    break;
                case Char:
                    addc(b,'\'');
                    addc(b,u.Char);
                    addc(b,'\'');
                    break;
                case Bool:
                    StringBuilder_append_cptr(b, u.Bool ? "true" : "false");
                    break;
                case Null:
                    if(!u.VoidPtr) StringBuilder_append_cptr(b, "null");
                    else throw("Null-type pointer is not 0");
                    break;
                case AutoarrUnitypePtr:
                    addc(b,'[');
                    Autoarr_foreach(((Autoarr_Unitype*)(u.VoidPtr)), e, ({
                        addc(b,' ');
                        AppendValue(e);
                        addc(b,',');
                    }));
                    Autoarr_remove(b);
                    addc(b,' ');
                    addc(b,']');
                    break;
                case HashtablePtr:
                    addc(b,'{');
                    addc(b,'\n');
                    __serialize(b,tabs+1,u.VoidPtr);
                    AppendTabs();
                    addc(b,'}');
                    break;
                default: dbg((u.type)); throw(ERR_WRONGTYPE); 
        }
    };
    
    Hashtable_foreach(dtsod, p, ({
        AppendTabs();
        StringBuilder_append_cptr(b,p.key);
        addc(b,':');
        addc(b,' ');
        AppendValue(p.value);
        addc(b,';');
        addc(b,'\n');
    }));
}

char* DtsodV24_serialize(Hashtable* dtsod){
    StringBuilder b=StringBuilder_create(STRB_BC,STRB_BL);
    __serialize(&b,0,dtsod);
    char* str=StringBuilder_build(&b);
    Autoarr_clear((&b));
    return str;
}
