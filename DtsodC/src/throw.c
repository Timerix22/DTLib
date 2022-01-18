#ifndef STDLIB
    #include "../cosmopolitan/cosmopolitan.h"
#else
    #include <stdio.h>
    #include <stdlib.h>
#endif
#include "err_t.h"

void throwstr(const char* errmesg){
    printf("\e[31mthrowed error: %s\e[0m\n",errmesg);
    exit(1); 
}

void throwerr(err_t err){
    printf("\e[31mthrowed error: err_t:%d\e[0m\n",err);
    exit(err); 
}