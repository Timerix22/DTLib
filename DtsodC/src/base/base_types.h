#pragma once

#include "std.h"
#include "errors.h"

typedef int8_t int8;
typedef uint8_t uint8;
typedef int16_t int16;
typedef uint16_t uint16;
typedef int32_t int32;
typedef uint32_t uint32;
typedef int64_t int64;
typedef uint64_t uint64;
typedef enum base_type{
    Null, Float, Double, Char, Bool,
    UInt8, Int8, UInt16, Int16, UInt32, Int32, UInt64, Int64, 
    UInt8Ptr, Int8Ptr, UInt16Ptr, Int16Ptr, UInt32Ptr, Int32Ptr, UInt64Ptr, Int64Ptr
} __attribute__ ((__packed__)) base_type;

const char* typename(base_type t);

int8 typesize(base_type type);

void AssignVoidToVoid(void* a, void*b, base_type type);
