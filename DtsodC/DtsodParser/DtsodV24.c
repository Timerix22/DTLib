#include "DtsodV24.h"

//returns UniNull if key not found
Unitype DtsodV24_get(Hashtable* dtsod, char* key){
    return Hashtable_get(dtsod, key);
}

//adds or sets value
void DtsodV24_addOrSet(Hashtable* dtsod, char* key, Unitype value){
    Unitype* val=Hashtable_getptr(dtsod, key);
    if(val) *val=value;
    else Hashtable_add(dtsod, key, value);
}

//checks for dtsod contains value or dont
bool DtsodV24_contains(Hashtable* dtsod, char* key){
    Unitype* val=Hashtable_getptr(dtsod, key);
    return val!=NULL;
}

//replaces value with UniNull if key exists in dtsod
bool DtsodV24_remove(Hashtable* dtsod, char* key){
    Unitype* val=Hashtable_getptr(dtsod, key);
    if (!val) return false;
    *val=UniNull;
    return true;
}
