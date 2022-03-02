#include "tests.h"
#include "../Autoarr/Autoarr.h"

static void printautoarr(Autoarr(uint16)* ar){
    printf("\e[94mAutoarr(uint16): %lu\n"
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

static void fillar(Autoarr(uint16)* ar){
    for (uint16 i=0;i<Autoarr_max_length(ar);i++)
        Autoarr_add(ar,i);
}
static void resetar(Autoarr(uint16)* ar){
    for (uint16 i=0;i<Autoarr_max_length(ar);i++)
        Autoarr_set(ar,i,Autoarr_max_length(ar)-i-1);
}

static void printallval(Autoarr(uint16)* ar){
    printf("\e[90m");
    for (uint16 i=0;i<Autoarr_length(ar);i++)
        printf("%u ",Autoarr_get(ar,i));
    printf("\n");
}

void test_autoarr(){
    optime("test_autoarr",1,({
        printf("\e[96m------------[test_autoarr]------------\n");
        Autoarr(uint16) ar=Autoarr_create(uint16,10,16);
        printf("\e[92mautoarr created\n");
        fillar(&ar);
        printf("\e[92mautoarr filled up\n");
        printautoarr(&ar);
        printallval(&ar);
        resetar(&ar);
        printf("\e[92mautoarr values reset\n");
        printallval(&ar);
        Autoarr_clear(((&ar)));
        printf("\e[92mautoarr cleared\n");
    }));
}
