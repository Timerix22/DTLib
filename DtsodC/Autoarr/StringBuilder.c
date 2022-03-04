#include "StringBuilder.h"

StringBuilder StringBuilder_create(uint16 max_blocks_count, uint16 max_block_length){
    return Autoarr_create(int8,max_blocks_count,max_block_length);
}

void StringBuilder_append_char(StringBuilder* b, char c){
    Autoarr_add(b,c);
}

void StringBuilder_append_cptr(StringBuilder* b, char* s){
    char c;
    while((c=*s++))
        Autoarr_add(b,c);
}

void StringBuilder_append_string(StringBuilder* b, string s){
    while(s.length>0){
        Autoarr_add(b,*s.ptr++);
        s.length--;
    }
}

void StringBuilder_append_int64(StringBuilder* b, int64 a){
    uint8 i=0;
    if(a==0){
        Autoarr_add(b,'0');
    }
    else if(a<0){
        Autoarr_add(b,'-');
        a=-a;
    }
    char buf[24];
    while(a!=0){
        buf[i++]='0'+a%10;
        a/=10;
    }
    string rev=string_reverse((string){buf,i});
    StringBuilder_append_string(b,rev);
}

void StringBuilder_append_uint64(StringBuilder* b, uint64 a){
    uint8 i=0;
    if(a==0){
        Autoarr_add(b,'0');
    }
    char buf[24];
    while(a!=0){
        buf[i++]='0'+a%10;
        a/=10;
    }
    string rev=string_reverse((string){buf,i});
    StringBuilder_append_string(b,rev);
}

void StringBuilder_append_double(StringBuilder* b, double a){
    char buf[32];
    sprintf(buf,32,"%f",a);
    StringBuilder_append_cptr(b,buf);
}

char* StringBuilder_build(StringBuilder* b){
    uint32 len=Autoarr_length(b);
    char* str=malloc(len+1);
    str[len]=0;
    for(uint32 i=0;i<len;i++)
        str[i]=Autoarr_get(b,i);
    return str;
}
