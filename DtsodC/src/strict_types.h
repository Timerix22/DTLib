#ifndef STDLIB
    #include "../cosmopolitan/cosmopolitan.h"
#else
    #include "stdint.h"
#endif
#include "err_t.h"

typedef int8_t int8;
typedef uint8_t uint8;
typedef int16_t int16;
typedef uint16_t uint16;
typedef int32_t int32;
typedef uint32_t uint32;
typedef int64_t int64;
typedef uint64_t uint64;

#ifndef __STRICT_TYPES_DEFINED
#define __STRICT_TYPES_DEFINED
typedef enum strict_type{
    Int8, Int16, Int32, Int64, Uint8, Uint16, Uint32, Uint64
} strict_type;

int8 GetTypeSize(strict_type type);

void AssignVoidToVoid(void* a, void*b, strict_type type);
#endif