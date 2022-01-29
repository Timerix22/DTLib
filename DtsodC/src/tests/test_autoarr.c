#include "tests.h"
#include "../base/Autoarr.h"

void ardesc(Autoarr* ar){
    printf("AUTOARR:%lu\n"
    "    type: %s\n"
    "    max_block_count: %u\n"
    "    curr_block_count: %u\n"
    "    max_block_length: %u\n"
    "    curr_block_length: %u\n"
    "    max_length: %u\n" 
    "    curr_length: %u\n",
    sizeof(Autoarr),
    typename(ar->type),
    ar->max_block_count,
    ar->curr_block_count,
    ar->max_block_length,
    ar->curr_block_length,
    ar->max_length,
    ar->curr_length);
}

void fillar(Autoarr* ar){
    for (uint16 i=0;i<ar->max_length;i++)
        Autoarr_add_uint16(ar,i);
    ardesc(ar);
}

void printar(Autoarr* ar){
    for (uint16 i=0;i<ar->max_length;i++)
        printf("%u ", Autoarr_get_uint16(&ar,i));
}

void testar(){
    Autoarr ar=Autoarr_create(10,16,UInt16);
    fillar(&ar);
    printar(&ar);
    Autoarr_free(&ar);
    ardesc(&ar);
}
