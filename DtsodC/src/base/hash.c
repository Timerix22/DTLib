#include "base.h"

uint32 hash(char *str){
    uint32 hash=5381;
    char c;
    while (c=*str++)
        hash=((hash << 5) + hash) + c; //hash=hash*33^c
    return hash;
}