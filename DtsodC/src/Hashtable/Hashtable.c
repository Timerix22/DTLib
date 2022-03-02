#include "Hashtable.h"

// amount of rows
#define HT_HEIN_MIN 0
#define HT_HEIN_MAX 5
static const uint16 HT_HEIGHTS[]={61,257,1021,4099,16381,65521};

#define ARR_BC 2
#define ARR_BL 8

Hashtable* Hashtable_create(){
    Hashtable* ht=malloc(sizeof(Hashtable));
    ht->hein=HT_HEIN_MIN;
    ht->rows=malloc(HT_HEIGHTS[HT_HEIN_MIN]*sizeof(Autoarr2(KeyValuePair)));
    for(uint16 i=0;i<HT_HEIGHTS[HT_HEIN_MIN];i++)
        ht->rows[i]=Autoarr2_create(KeyValuePair,ARR_BC,ARR_BL);
    return ht;
}

void Hashtable_free(Hashtable* ht){
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein];i++){
        Autoarr2_KeyValuePair_clear(ht->rows+i);
    }
    free(ht->rows);
    free(ht);
}

uint32 Hashtable_height(Hashtable* ht){ return HT_HEIGHTS[ht->hein]; }


void Hashtable_expand(Hashtable* ht){
    if(ht->hein>=HT_HEIN_MAX) throw(ERR_MAXLENGTH);
    Autoarr2(KeyValuePair)* newrows=malloc(HT_HEIGHTS[++ht->hein]*sizeof(Autoarr2(KeyValuePair)));
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein];i++)
        newrows[i]=Autoarr2_create(KeyValuePair,ARR_BC,ARR_BL);
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein-1];i++){
        Autoarr2(KeyValuePair)* ar=ht->rows+i;
        uint32 arlen=Autoarr2_length(ar);
        for(uint16 k=0;k<arlen;k++){
            KeyValuePair p=Autoarr2_get(ar,k);
            uint16 newrown=ihash(p.key)%HT_HEIGHTS[ht->hein];
            Autoarr2(KeyValuePair)* newar=newrows+newrown;
            Autoarr2_add(newar,p);
        }
        Autoarr2_clear(ar);
    }
    free(ht->rows);
    ht->rows=newrows;
}

Autoarr2(KeyValuePair)* getrow(Hashtable* ht, char* key, bool can_expand){
    Autoarr2(KeyValuePair)* ar=ht->rows+ihash(key)%HT_HEIGHTS[ht->hein];
    if(can_expand && Autoarr2_length(ar)==Autoarr2_max_length(ar))
        optime("expand",1,(Hashtable_expand(ht)));
    ar=ht->rows+ihash(key)%HT_HEIGHTS[ht->hein];
    return ar;
}


void Hashtable_add_pair(Hashtable* ht, KeyValuePair p){
    Autoarr2_add(getrow(ht,p.key,true),p);
}
void Hashtable_add(Hashtable* ht, char* key, Unitype u){
    Hashtable_add_pair(ht,KVPair(key,u));
}

//returns null or pointer to value in hashtable
Unitype* Hashtable_getptr(Hashtable* ht, char* key){
    Autoarr2(KeyValuePair)* ar=getrow(ht,key,false);
    uint32 arlen=Autoarr2_length(ar);
    for(uint32 i=0;i<arlen;i++){
        KeyValuePair* p=Autoarr2_getptr(ar,i);
        if(charbuf_compare(key,p->key)) return &p->value;
    }
    return NULL;
}

Unitype Hashtable_get(Hashtable* ht, char* key){
    Autoarr2(KeyValuePair)* ar=getrow(ht,key,false);
    uint32 arlen=Autoarr2_length(ar);
    for(uint32 i=0;i<arlen;i++){
        KeyValuePair p=Autoarr2_get(ar,i);
        if(charbuf_compare(key,p.key)) return p.value;
    }
    return UniNull;
}
KeyValuePair Hashtable_get_pair(Hashtable* ht, char* key){
    return KVPair(key,Hashtable_get(ht,key));
}
bool Hashtable_try_get(Hashtable* ht, char* key, Unitype* output){
    Unitype u=Hashtable_get(ht,key);
    *output=u;
    return u.type!=Null;
}

/* void Hashtable_set_pair(Hashtable* ht, KeyValuePair p){
    if(Hashtable_try_get(ht,p.key, NULL)){

    }
}
void Hashtable_set(Hashtable* ht, char* key, Unitype u){ Hashtable_set_pair(ht,KVPair(key,u)); } */
