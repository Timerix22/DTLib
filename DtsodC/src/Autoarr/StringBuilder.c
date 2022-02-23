#include "StringBuilder.h"

StringBuilder StringBuilder_create(uint16 max_blocks_count, uint16 max_block_length){
    return Autoarr2_create(int8,max_blocks_count,max_block_length);
}

void StringBuilder_append(StringBuilder* b, char c){
    Autoarr2_add(b,c);
}

void StringBuilder_append_str(StringBuilder* b, char* s){
    char c;
    while((c=*s++))
        Autoarr2_add(b,c);
}

char* StringBuilder_build(StringBuilder* b){
    uint32 len=Autoarr2_length(b);
    char* str=malloc(len+1);
    str[len]=0;
    for(uint32 i=0;i<len;i++)
        str[i]=Autoarr2_get(b,i);
    return str;
}
