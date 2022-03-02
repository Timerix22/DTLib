#include "Autoarr2.h"

define_Autoarr2(uint8)
define_Autoarr2(int8)
define_Autoarr2(uint16)
define_Autoarr2(int16)
define_Autoarr2(uint32)
define_Autoarr2(int32)
define_Autoarr2(uint64)
define_Autoarr2(int64)
define_Autoarr2(float)
define_Autoarr2(double)
define_Autoarr2(Unitype)

//right func to clear array of unitype values
void Autoarr2_Unitype_clear(Autoarr2(Unitype)* ar){
    for(uint32 blockI=0;blockI<ar->blocks_count-1;blockI++)
        for(uint32 elemI=0;elemI<ar->max_block_length;elemI++)
            Unitype_free(ar->values[blockI][elemI]);
    for(uint32 elemI=0;elemI<ar->block_length;elemI++)
        Unitype_free(ar->values[ar->blocks_count-1][elemI]);
    Autoarr2_clear(ar);
}
