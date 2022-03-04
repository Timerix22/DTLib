#include "DtsodV24.h"
#include "../Autoarr/StringBuilder.h"

#define STRB_BC 64
#define STRB_BL 1024

#define addc(B,C) StringBuilder_append_char(B,C)

void __serialize(StringBuilder* b, uint8 tabs, Hashtable* dtsod){
    
    void AppendTabs(){
        for(uint8 t; t<tabs; t++)
            addc('\t');
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
                    addc(b,'d');
                    break;
                case String:
                    addc(b,'"');
                    StringBuilder_append_cptr(b,u.VoidPtr);
                    addc(b,'"');
                    break;
                case Char:
                    addc(b,'\'');
                    addc(b,u.Char);
                    addc(b,'\'');
                    break;
                case AutoarrUnitypePtr:
                    addc(b,'[');
                    Autoarr_foreach(((Autoarr_Unitype)(u.VoidPtr)), e, ({
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
                default: throw(ERR_WRONGTYPE); 
        }
    };
    
    Hashtable_foreach(dtsod, p, ({
        AppendTabs();
        StringBuilder_append_cptr(p.key);
        addc(b,':');
        addc(b,' ');
        AppendValue(p.value);
        addc(b,';');
        addc(b,'\n');
    }));
}

char* DtsodV24_serialize(Hashtable* dtsod){
    StringBuilder b=StringBuilder_create(STRB_BC,STR_BL); //65536 max!
    __serialize(&b,0,dtsod);
    return StringBuilder_build(&b);
}
