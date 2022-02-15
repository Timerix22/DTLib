#include "tests.h"
#include "../Autoarr/Autoarr2.h"

static void printautoarr(Autoarr2(uint16)* ar){
    printf("\e[94mAUTOARR:%lu\n"
        "  max_blocks_count: %u\n"
        "  blocks_count: %u\n"
        "  max_block_length: %u\n"
        "  block_length: %u\n"
        "  max_length: %u\n" 
        "  length: %u\n",
        sizeof(Autoarr2(uint16)),
        ar->max_blocks_count,
        ar->blocks_count,
        ar->max_block_length,
        ar->block_length,
        Autoarr2_max_length(ar),
        Autoarr2_length(ar));
}

static void fillar(Autoarr2(uint16)* ar){
    for (uint16 i=0;i<Autoarr2_max_length(ar);i++)
        Autoarr2_add(ar,i);
}
static void resetar(Autoarr2(uint16)* ar){
    for (uint16 i=0;i<Autoarr2_max_length(ar);i++)
        Autoarr2_set(ar,i,Autoarr2_max_length(ar)-i-1);
}

static void printallval(Autoarr2(uint16)* ar){
    printf("\e[90m");
    for (uint16 i=0;i<Autoarr2_length(ar);i++)
        printf("%u ",Autoarr2_get(ar,i));
    printf("\n");
}

void test_autoarr2(){
    printf("\e[96m------------[test_autoarr2]-------------\n");
    Autoarr2(uint16) ar=Autoarr2_create(uint16,10,16);
    printf("\e[92mautoarr created\n");
    fillar(&ar);
    printf("\e[92mautoarr filled up\n");
    printautoarr(&ar);
    printallval(&ar);
    resetar(&ar);
    printf("\e[92mautoarr values reset\n");
    printallval(&ar);
    Autoarr2_clear(((&ar)));
    printf("\e[92mautoarr cleared\n");
}
