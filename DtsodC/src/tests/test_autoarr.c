#include "tests.h"
#include "../Autoarr/Autoarr.h"

void printautoarr(Autoarr* ar){
    printf("\e[94mAUTOARR:%lu\n"
        "  type: %s\n"
        "  max_block_count: %u\n"
        "  curr_block_count: %u\n"
        "  max_block_length: %u\n"
        "  curr_block_length: %u\n"
        "  max_length: %u\n" 
        "  curr_length: %u\n",
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
}

void printallval(Autoarr* ar){
    for (uint16 i=0;i<ar->max_length;i++)
        printf("%u ", Autoarr_get_uint16(ar,i));
}

void test_autoarr(){
    printf("\e[96m------------[test_autoarr]-------------\n");
    Autoarr ar=Autoarr_create(10,16,UInt16);
    printf("\e[92mautoarr created\n\e[90m");
    fillar(&ar);
    printallval(&ar);
    printf("\n\e[92mautoarr filled up\n");
    printautoarr(&ar);
    Autoarr_clear(&ar);
    printf("\e[92mautoarr cleared\n");
}
