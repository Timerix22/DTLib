#include "!headers.h"
#include "strict_types.h"
#include "errors.h"

int8 GetTypeSize(strict_type type){
    int8 type_size=0;
    switch (type)
    {
        case Int8: type_size=8; break;
        case Uint8: type_size=8; break;
        case Int16: type_size=16; break;
        case Uint16: type_size=16; break;
        case Int32: type_size=32; break;
        case Uint32: type_size=32; break;
        case Int64: type_size=64; break;
        case Uint64: type_size=64; break;
        default: throwerr(ERR_WRONGTYPE);
    }
    return type_size;
}

void AssignVoidToVoid(void* a, void*b, strict_type type){
    switch (type)
    {
        case Int8: *((int8*)a)=*((int8*)b); break; 
        case Uint8: *((uint8*)a)=*((uint8*)b); break; 
        case Int16: *((int16*)a)=*((int16*)b); break;
        case Uint16: *((uint16*)a)=*((uint16*)b); break; 
        case Int32: *((int32*)a)=*((int32*)b); break;
        case Uint32: *((uint32*)a)=*((uint32*)b); break; 
        case Int64: *((int64*)a)=*((int64*)b); break;
        case Uint64: *((uint64*)a)=*((uint64*)b); break; 
        default: throwerr(ERR_WRONGTYPE);
    }
}