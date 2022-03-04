#pragma once

#include "Autoarr_declare.h"
#include "Autoarr_define.h"

declare_Autoarr(uint8)
declare_Autoarr(int8)
declare_Autoarr(uint16)
declare_Autoarr(int16)
declare_Autoarr(uint32)
declare_Autoarr(int32)
declare_Autoarr(uint64)
declare_Autoarr(int64)
declare_Autoarr(float)
declare_Autoarr(double)
declare_Autoarr(Unitype)

//right func to clear array of unitype values
void Autoarr_Unitype_clear(Autoarr(Unitype)* ar);

#define Autoarr_foreach(ar,elem,codeblock)({\
    if(ar->blocks_count>0) {\
        typeof(**ar->values) elem;\
        for(uint32 blockI=0;blockI<ar->blocks_count-1;blockI++)\
            for(uint32 elemI=0;elemI<ar->max_block_length;elemI++){dbg(5);\
                elem=ar->values[blockI][elemI];\
                (codeblock);\
            }\
        for(uint32 elemI=0;elemI<ar->block_length;elemI++){\
            elem=ar->values[ar->blocks_count-1][elemI];\
            (codeblock);\
        }\
    }\
})
