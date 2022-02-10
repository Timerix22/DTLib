#include "Hashtable.h"

Hashtable Hashtable_create(uint16 height,my_type type){
    Hashtable h={
        .type=type,
        .height=height,
        .rows=malloc(height*sizeof(Autoarr))
    };
    for(uint16 i=0;i<height;i++)
        h.rows[i]=Autoarr_create(100,8,type);
    return h;
}

void Hashtable_clear(Hashtable* ht){
    for(uint16 i=0;i<ht->height;i++)
        Autoarr_clear((Autoarr*)(ht->rows+i));
    free(ht->rows);
    ht->type=Null;
} 

void Hashtable_add_uni(Hashtable* ht,hash_t hash, Unitype val){
    
}

