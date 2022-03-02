#include "KeyValuePair.h"

define_Autoarr2(KeyValuePair)


//proper way to clear a KVP
void KeyValuePair_free(KeyValuePair p){
    free(p.key);
    Unitype_free(p.value);
}

//func for  KVP array clearing
void Autoarr2_KeyValuePair_clear(Autoarr2_KeyValuePair* ar){
    for(uint16 blockI=0; blockI < ar->blocks_count-1; blockI++)
        for(uint16 elemI=0; elemI < ar->max_block_length; elemI++)
            KeyValuePair_free(ar->values[blockI][elemI]);
    for(uint16 elemI=0; elemI < ar->block_length; elemI++)
        KeyValuePair_free(ar->values[ar->blocks_count-1][elemI]);
    Autoarr2_clear(ar);
}
