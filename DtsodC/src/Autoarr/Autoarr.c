#include "Autoarr.h"

define_Autoarr(uint8)
define_Autoarr(int8)
define_Autoarr(uint16)
define_Autoarr(int16)
define_Autoarr(uint32)
define_Autoarr(int32)
define_Autoarr(uint64)
define_Autoarr(int64)
define_Autoarr(float)
define_Autoarr(double)
define_Autoarr(Unitype)

//right func to clear array of unitype values
void Autoarr_Unitype_clear(Autoarr(Unitype)* ar){
    for(uint32 blockI=0;blockI<ar->blocks_count-1;blockI++)
        for(uint32 elemI=0;elemI<ar->max_block_length;elemI++)
            Unitype_free(ar->values[blockI][elemI]);
    for(uint32 elemI=0;elemI<ar->block_length;elemI++)
        Unitype_free(ar->values[ar->blocks_count-1][elemI]);
    Autoarr_clear(ar);
}
