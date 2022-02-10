#include "hash.h"

uint32 ihash(char *str){
    uint32 hash=5381;
    char c;
    while(c=*(str++))
        hash=((hash<<5)+hash)+c; //hash=hash*33^c
    return hash;
}

uint64 lhash(char* str){
    uint64 hash = 0;
    int c;
    while (c=*(str++))
        hash=c+(hash<<6)+(hash<<16)-hash;
    return hash;
}
