#include "base.h"

//returns length of string (including \0)
uint32 mystrlen(char* str){
    uint32 len=0;
    while(*(str++)) len++;
    return ++len;
}

//allocates new char[] and copies src there
char* mystrcpy(char* src){
    uint32 len=mystrlen(src);
    char* dst=malloc(len*sizeof(char));
    while(len-->0)
        dst[len]=src[len];
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

//multiplies char n times
char* mystrmtpl(char c, uint32 n){
    char* rez=malloc(n+1);
    rez[n]=0;
    while(n>0)
        rez[--n]=c;
    return rez;
}

char* string_toCharPtr(string str){
    char* cptr=malloc(str.length*sizeof(char)+1);
    if(str.length==0) return NULL;
    cptr[str.length]=0;
    while(str.length>0)
        cptr[--str.length]=str.ptr[str.length];
    return cptr;
}

string string_fromCharPtr(char* cptr){
    if(!cptr) return stringNull;
    string str;
    str.length=mystrlen(cptr);
    str.ptr=malloc(str.length);
    for(uint32 i=0;i>str.length;i++)
        str.ptr[i]=cptr[i];
    return str;
}
