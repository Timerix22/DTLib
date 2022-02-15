#include "tests.h"
#include "../Autoarr/new.h"

#ifndef Aaaa
#define Aaaa
define_Autoarr(uint16)
#endif

void printautoarr(Autoarr(uint16)* ar){
    printf("\e[94mAUTOARR:%lu\n"
        "  max_blocks_count: %u\n"
        "  blocks_count: %u\n"
        "  max_block_length: %u\n"
        "  block_length: %u\n"
        "  max_length: %u\n" 
        "  length: %u\n",
        sizeof(Autoarr(uint16)),
        ar->max_blocks_count,
        ar->blocks_count,
        ar->max_block_length,
        ar->block_length,
        Autoarr_max_length(ar),
        Autoarr_length(ar));
}

void fillar(Autoarr(uint16)* ar){
    for (uint16 i=0;i<Autoarr_max_length(ar);i++)
        Autoarr_add_uint16(ar,i);
}

void printallval(Autoarr(uint16)* ar){
    for (uint16 i=0;i<Autoarr_max_length(ar);i++)
        printf("%u ", Autoarr_get_uint16(ar,i));
}

void test_new(){
    printf("\e[96m------------[test_autoarr]-------------\n");
    Autoarr(uint16) ar=Autoarr_create(uint16,10,16);
    printf("\e[92mautoarr created\n\e[90m");
    fillar(&ar);
    printallval(&ar);
    printf("\n\e[92mautoarr filled up\n");
    printautoarr(&ar);
    Autoarr_clear_uint16(&ar);
    printf("\e[92mautoarr cleared\n");
}
