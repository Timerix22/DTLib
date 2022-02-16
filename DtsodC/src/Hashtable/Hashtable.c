#include "Hashtable.h"

define_Autoarr2(KeyValuePair)

Hashtable* Hashtable_create(){
    Hashtable* ht=malloc(sizeof(Hashtable));
    ht->hein=HT_HEIGHTS[0];
    ht->rows=malloc(HT_HEIGHTS[0]*sizeof(Autoarr2(KeyValuePair)));
    for(uint16 i;i<HT_HEIGHTS[0];i++)
        ht->rows[i]=Autoarr2_create(KeyValuePair,4,16);
    return ht;
}

static Autoarr2(KeyValuePair)* getrow(Hashtable* ht,char* key){
    uint16 rown=HT_HEIGHTS[ht->hein]%ihash(key);
    if(rown>=HT_HEIGHTS[ht->hein]){
        ht->rows=realloc(ht->rows,HT_HEIGHTS[++ht->hein]*sizeof(Autoarr2(KeyValuePair)));
        for(uint16 i=HT_HEIGHTS[ht->hein-1];i<HT_HEIGHTS[ht->hein];i++)
            ht->rows[i]=Autoarr2_create(KeyValuePair,4,16);
    }
    return ht->rows+rown;
}

void Hashtable_free(Hashtable* ht){
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein];i++)
        Autoarr2_clear(((Autoarr2(KeyValuePair)*)(ht->rows+i)));
    free(ht->rows);
    free(ht);
}

void Hashtable_add_pair(Hashtable* ht, KeyValuePair pair){
    Autoarr2_add(getrow(ht,pair.key),pair);
}

//returns length of string (including \0)
uint32 mystrlen(char* str){
    uint32 len=0;
    while(*(str++)) len++;
    return len++;
}

//allocates new char[] and copies src there
char* mystrcpy(char* src){
    uint32 len=mystrlen(src);
    char* dst=malloc(len*sizeof(char));
    while(--len<0)
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

void Hashtable_add(Hashtable* ht, char* key, Unitype value){
    Hashtable_add_pair(ht,(KeyValuePair){key,value});
}

Unitype Hashtable_get(Hashtable* ht, char* key){
    Autoarr2(KeyValuePair)* ar=getrow(ht,key);
    uint32 arlen=Autoarr2_length(ar);
    for(uint32 i=0;i<arlen;i++){
        KeyValuePair p=Autoarr2_get(ar,i);
        if(mystrcmp(key,p.key)) return p.value;
    }
    return (Unitype){.type=Null,.VoidPtr=NULL};
}

KeyValuePair Hashtable_get_pair(Hashtable* ht, char* key){
    return (KeyValuePair){
        .key=key,
        .value=Hashtable_get(ht,key)
    };
}

bool Hashtable_try_get(Hashtable* ht, char* key,Unitype* output){
    Unitype u=Hashtable_get(ht,key);
    *output=u;
    return u.type==Null;
}
