#include "tests.h"


void printuni(Unitype v){
    switch (v.type) {
        case Null: printf("{%s}",typename(v.type));break;
        case Double: printf("{%s:%lf}",typename(v.type),v.Double);break;
        case Float: printf("{%s:%f}",typename(v.type),v.Float);break;
        case Char: printf("{%s:%c}",typename(v.type),v.Int8);break;
        case UInt8: 
        case UInt16: printf("{%s:%u}",typename(v.type),v.UInt16);break;
        case UInt32: 
        case UInt64: printf("{%s:%lu}",typename(v.type),v.UInt64);break;
        case Bool: 
        case Int8: 
        case Int16: printf("{%s:%d}",typename(v.type),v.Int16);break;
        case Int32: 
        case Int64: printf("{%s:%ld}",typename(v.type),v.Int64);break;
        case Int8Ptr: 
        case UInt8Ptr: 
        case Int16Ptr: 
        case UInt16Ptr: 
        case Int32Ptr: 
        case UInt32Ptr: 
        case Int64Ptr: 
        case UInt64Ptr: printf("{%s:%p}",typename(v.type),v.VoidPtr);break;
        default: throw(ERR_WRONGTYPE);break;
    }
}

void test_all(void){
    test_searchtree();
    test_autoarr();
    test_autoarr2();
    printf("\e[96m---------------------------------------\n");
}