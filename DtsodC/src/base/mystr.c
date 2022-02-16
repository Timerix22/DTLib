#include "base.h"

//returns length of string (including \0)
uint32 mystrlen(char* str){
    uint32 len=0;
    while(*(str++)) len++;
    return ++len;
}

//allocates new char[] and copies src there
char* mystrcpy(char* src){
    uint32 len=mystrlen(src);dbg(len);
    char* dst=malloc(len*sizeof(char));
    while(len-->0)
        dst[len]=src[len];
    printf("dst: %s\n",dst);
    return dst;
}

//compares two strings, NullPtr-friendly
bool mystrcmp(char* key0, char* key1){
    if(!key0) return key1 ? 0 : 1;
    else if(!key1) return 0;
    while(*key0&&*key1){
        if(*key0!=*key1) return 0;
        key0++;
        key1++;
    }
    return 1;
}
