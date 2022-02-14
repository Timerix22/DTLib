#include "Hashtable.h"
/*
Hashtable Hashtable_create(uint16 height){
    Hashtable h={
        .height=height,
        .rows=malloc(height*sizeof(Autoarr))
    };
    return h;
}

void Hashtable_clear(Hashtable* ht){
    for(uint16 i=0;i<ht->height;i++)
        Autoarr_clear(ht->rows+i);
} 

void Hashtable_add_kvpair(Hashtable* ht, KeyValuePair pair){
    uint16 i=ht->height%ihash(pair.key);
    Autoarr_add_kvpair(ht->rows[i],pair);
}

void Hashtable_add(Hashtable* ht, char* key, Unitype value){
    Hashtable_add_kvpair(ht,(KeyValuePair){key,value});
}
*/