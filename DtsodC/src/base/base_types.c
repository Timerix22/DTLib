#include "std.h"
#include "base_types.h"
#include "errors.h"

const char* typename(base_type t){
    switch (t) {
        case Null: return "Null";
        case Int8: return "Int8";
        case UInt8: return "UInt8";
        case Int16: return "Int16";
        case UInt16: return "UInt16";
        case Int32: return "Int32";
        case UInt32: return "UInt32";
        case Int64: return "Int64";
        case UInt64: return "UInt64";
        default: throw(ERR_WRONGTYPE); return "EEEEEE";
    }
}

int8 typesize(base_type type){
    int8 type_size=0;
    switch (type)
    {
        case Int8: type_size=8; break;
        case UInt8: type_size=8; break;
        case Int16: type_size=16; break;
        case UInt16: type_size=16; break;
        case Int32: type_size=32; break;
        case UInt32: type_size=32; break;
        case Int64: type_size=64; break;
        case UInt64: type_size=64; break;
        default: throw(ERR_WRONGTYPE);
    }
    return type_size;
}

void AssignVoidToVoid(void* a, void*b, base_type type){
    switch (type)
    {
        case Int8: *((int8*)a)=*((int8*)b); break; 
        case UInt8: *((uint8*)a)=*((uint8*)b); break; 
        case Int16: *((int16*)a)=*((int16*)b); break;
        case UInt16: *((uint16*)a)=*((uint16*)b); break; 
        case Int32: *((int32*)a)=*((int32*)b); break;
        case UInt32: *((uint32*)a)=*((uint32*)b); break; 
        case Int64: *((int64*)a)=*((int64*)b); break;
        case UInt64: *((uint64*)a)=*((uint64*)b); break; 
        default: throw(ERR_WRONGTYPE);
    }
}