#include "new.h"
#include "../Autoarr/new.h"
#include "../base/base.h"

define_Autoarr(uint16)

void test_new(){
    printf("\e[96m------------[test_autoarr]-------------\n");
    Autoarr(uint16) ar=Autoarr_create(uint16,10,16);
    printf("\e[92mautoarr created\n\e[90m");
    fillar(&ar);
    printallval(&ar);
    printf("\n\e[92mautoarr filled up\n");
    printautoarr(&ar);
    Autoarr_clear(&ar);
    printf("\e[92mautoarr cleared\n");
}