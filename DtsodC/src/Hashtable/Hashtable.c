#include "Hashtable.h"

define_Autoarr2(KeyValuePair)

// amount of rows
static const uint16 HT_HEIGHTS[]={61,631,3889,19441,65521};

#define ARR_BC 16
#define ARR_BL 128

Hashtable* Hashtable_create(){
    Hashtable* ht=malloc(sizeof(Hashtable));
    //ht->hein=0;//    
    ht->hein=1;
    ht->rows=malloc(HT_HEIGHTS[ht->hein]*sizeof(Autoarr2(KeyValuePair)));
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein];i++)
        ht->rows[i]=Autoarr2_create(KeyValuePair,ARR_BC,ARR_BL);
    return ht;
}

void Hashtable_free(Hashtable* ht){
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein];i++){
        Autoarr2(KeyValuePair)* ar=(Autoarr2(KeyValuePair)*)(ht->rows+i);
        for(uint32 i=0;i<Autoarr2_length(ar);i++)
            free(Autoarr2_getptr(ar,i)->key);
        Autoarr2_clear(ar);
    }
    free(ht->rows);
    free(ht);
}

uint32 Hashtable_height(Hashtable* ht){ return HT_HEIGHTS[ht->hein]; }


void Hashtable_resize(Hashtable* ht){printf("RESIZE\n");
    Autoarr2(KeyValuePair)* newrows=malloc(HT_HEIGHTS[++ht->hein]*sizeof(Autoarr2(KeyValuePair)));
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein];i++)
        ht->rows[i]=Autoarr2_create(KeyValuePair,ARR_BC,ARR_BL);
    for(uint16 i=0;i<HT_HEIGHTS[ht->hein-1];i++){
        Autoarr2(KeyValuePair)* ar=ht->rows+i;
        for(uint16 k=0;k<Autoarr2_length(ar);k++){
            KeyValuePair p=Autoarr2_get(ar,k);
            uint16 newrown=ihash(p.key)%HT_HEIGHTS[ht->hein];
            Autoarr2(KeyValuePair)* newar=newrows+newrown;
            Autoarr2_add(newar,p);
        }
    }
    ht->rows=newrows;
}

Autoarr2(KeyValuePair)* getrow(Hashtable* ht, char* key){
    uint16 rown=ihash(key)%HT_HEIGHTS[ht->hein];
    //if(rown>=HT_HEIGHTS[ht->hein])
    //    Hashtable_resize(ht);
    return ht->rows+rown;
}

//copies string and value to new KeyValuePair
KeyValuePair cpair(char* key, Unitype value){
    return (KeyValuePair){.key=mystrcpy(key),.value=value};
}


void Hashtable_add_pair(Hashtable* ht, KeyValuePair p){
    Autoarr2_add(getrow(ht,p.key),p);
}
void Hashtable_add(Hashtable* ht, char* key, Unitype u){
    Hashtable_add_pair(ht,cpair(key,u));
}

//returns null or pointer to value in hashtable
Unitype* Hashtable_getptr(Hashtable* ht, char* key){
    Autoarr2(KeyValuePair)* ar=getrow(ht,key);
    uint32 arlen=Autoarr2_length(ar);
    for(uint32 i=0;i<arlen;i++){
        KeyValuePair* p=Autoarr2_getptr(ar,i);
        if(mystrcmp(key,p->key)) return &p->value;
    }
    return NULL;
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
    return cpair(key,Hashtable_get(ht,key));
}
bool Hashtable_try_get(Hashtable* ht, char* key, Unitype* output){
    Unitype u=Hashtable_get(ht,key);
    *output=u;
    return u.type==Null;
}

void Hashtable_set_pair(Hashtable* ht, KeyValuePair p){
    if(Hashtable_try_get(ht,p.key, NULL)){

    }
}
void Hashtable_set(Hashtable* ht, char* key, Unitype u){ Hashtable_set_pair(ht,cpair(key,u)); }
