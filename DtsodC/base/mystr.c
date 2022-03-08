#include "base.h"

//returns length of string (including \0)
uint32 mystrlen(char* str){
    uint32 len=0;
    while(*(str++)) len++;
    return ++len;
}

//allocates new char[] and copies src there
char* charbuf_copy(char* src){
    uint32 len=mystrlen(src);
    char* dst=malloc(len*sizeof(char));
    while(len-->0)
        dst[len]=src[len];
    return dst;
}

//compares two char buffers, NullPtr-friendly
bool charbuf_compare(char* key0, char* key1){
    if(!key0) return key1 ? false : true;
    if(!key1) return false;
    while(*key0&&*key1)
        if(*key0++ != *key1++) 
            return false;
    return true;
}

//multiplies char n times
char* char_multiply(char c, uint32 n){
    char* rez=malloc(n+1);
    rez[n]=0;
    while(n-->0)
        rez[n]=c;
    return rez;
}

//copies str content to new char pointer value (adding '\0' at the end)
char* string_cpToCharPtr(string str){
    char* cptr=malloc(str.length*sizeof(char)+1);
    if(str.length==0) return NULL;
    cptr[str.length]=0;
    while(str.length-->0)
        cptr[str.length]=str.ptr[str.length];
    return cptr;
}

//copies cptr content (excluding '\0' at the end) to new string
string string_cpFromCharPtr(char* cptr){
    if(!cptr) return stringNull;
    string str;
    str.length=mystrlen(cptr)-1;
    str.ptr=malloc(str.length);
    for(uint32 i=0;i<str.length;i++)
        str.ptr[i]=cptr[i];
    return str;
}

//compares two strings, NullPtr-friendly
bool string_compare(string str0, string str1){
    if(str0.length!=str1.length) return false;
    if(!str0.ptr) return str1.ptr ? false : true;
    else if(!str1.ptr) return false;
    while(str0.length-->0)
        if(*str0.ptr++ != *str1.ptr++) 
            return false;
    return true;
}

//creates new string which is reversed variant of <s>
string string_reverse(string s){
    string r={malloc(s.length), s.length};
    for(uint32 i=0; i<s.length; i++)
        r.ptr[i]=s.ptr[s.length-i-1];
    return r;
}
