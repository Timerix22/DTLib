#include "types.h"
#include "errors.h"

const char* typename(my_type t){
    switch (t) {
        case Null: return "Null";
        case Double: return "Double";
        case Float: return "Float";
        case Bool: return "Bool";
        case Char: return "Char";
        case Int8: return "Int8";
        case UInt8: return "UInt8";
        case Int16: return "Int16";
        case UInt16: return "UInt16";
        case Int32: return "Int32";
        case UInt32: return "UInt32";
        case Int64: return "Int64";
        case UInt64: return "UInt64";
        case Int8Ptr: return "Int8Ptr";
        case UInt8Ptr: return "UInt8Ptr";
        case Int16Ptr: return "Int16Ptr";
        case UInt16Ptr: return "UInt16Ptr";
        case Int32Ptr: return "Int32Ptr";
        case UInt32Ptr: return "UInt32Ptr";
        case Int64Ptr: return "Int64Ptr";
        case UInt64Ptr: return "UInt64Ptr";
        case UniversalType: return "Unitype";
        case CharPtr: return "CharPtr";
case STNodePtr: return "STNodePtr";
case HashtablePtr: return "HashtablePtr";
case AutoarrInt8Ptr: return "AutoarrInt8Ptr";
case AutoarrUInt8Ptr: return "AutoarrUInt8Ptr";
case AutoarrInt16Ptr: return "AutoarrInt16Ptr";
case AutoarrUInt16Ptr: return "AutoarrUInt16Ptr";
case AutoarrInt32Ptr: return "AutoarrInt32Ptr";
case AutoarrUInt32Ptr: return "AutoarrUInt32Ptr";
case AutoarrInt64Ptr: return "AutoarrInt64Ptr";
case AutoarrUInt64Ptr: return "AutoarrUInt64Ptr";
case AutoarrUnitypePtr: return "AutoarrUnitypePtr";
case AutoarrKVPairPtr: return "AutoarrKVPairPtr";
        default: throw(ERR_WRONGTYPE); return "ERROR";
    }
}

int8 typesize(my_type type){
    switch (type){
        case Null: return 0;
        case Double: return sizeof(double);
        case Float: return sizeof(float);
        case Bool: return sizeof(bool);
        case Char:
        case Int8: 
        case UInt8: return 1;
        case Int16: 
        case UInt16: return 2;
        case Int32: 
        case UInt32: return 4;
        case Int64:
        case UInt64: return 8;
        case Int8Ptr: 
        case UInt8Ptr: 
        case Int16Ptr: 
        case UInt16Ptr: 
        case Int32Ptr: 
        case UInt32Ptr: 
        case Int64Ptr: 
        case UInt64Ptr: return sizeof(void*);
        case UniversalType: return "Unitype";
        default: throw(ERR_WRONGTYPE); return -1;
    }
}
