#include "!headers.h"
#include "errors.h"

void throwstr(const char* errmesg){
    printf("\e[31mthrowed error: %s\e[0m\n",errmesg);
    exit(1); 
}

void throwerr(err_t err){
    printf("\e[31mthrowed error: err_t:%d\e[0m\n",err);
    exit(err); 
}