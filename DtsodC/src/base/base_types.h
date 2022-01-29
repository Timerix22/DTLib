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
    Null,Int8, Int16, Int32, Int64, UInt8, UInt16, UInt32, UInt64
} base_type;

const char* typename(base_type t);

int8 typesize(base_type type);

void AssignVoidToVoid(void* a, void*b, base_type type);
