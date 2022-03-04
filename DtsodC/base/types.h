#pragma once

#include "std.h"

typedef int8_t int8;
typedef uint8_t uint8;
typedef int16_t int16;
typedef uint16_t uint16;
typedef int32_t int32;
typedef uint32_t uint32;
typedef int64_t int64;
typedef uint64_t uint64;
typedef enum my_type{
    Null, Float, Double, Char, Bool,
    UInt8, Int8, UInt16, Int16, UInt32, Int32, UInt64, Int64, 
    UInt8Ptr, Int8Ptr, UInt16Ptr, Int16Ptr, UInt32Ptr, Int32Ptr, UInt64Ptr, Int64Ptr,
    CharPtr, STNodePtr, HashtablePtr,
    UniversalType,
    AutoarrInt8Ptr, AutoarrUInt8Ptr, AutoarrInt16Ptr, AutoarrUInt16Ptr, 
    AutoarrInt32Ptr, AutoarrUInt32Ptr, AutoarrInt64Ptr, AutoarrUInt64Ptr,
    AutoarrUnitypePtr, AutoarrKVPairPtr
} __attribute__ ((__packed__)) my_type;

//returns type name
const char* typename(my_type t);

// returns size of type in bytes
int8 typesize(my_type type);

// can store any base type
typedef struct Unitype{
    my_type type; 
    union {
        int64 Int64;
        uint64 UInt64;
        double Double;
        char Char;
        bool Bool;
        void* VoidPtr;
    };
} Unitype;

static const Unitype UniNull={Null,.VoidPtr=NULL};
static const Unitype UniTrue={Bool,.Bool=true};
static const Unitype UniFalse={Bool,.Bool=false};

#define Uni(TYPE,VAL) (Unitype){.type=TYPE,.TYPE=VAL}
#define UniPtr(TYPE,VAL) (Unitype){.type=TYPE,.VoidPtr=VAL}

//frees VoidPtr value or does nothing if type isn't pointer
void Unitype_free(Unitype u);
void printuni(Unitype v);
