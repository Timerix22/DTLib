#if __cplusplus
extern "c" {
#endif

#pragma once
#include "../base/base.h"
#include "../Hashtable/Hashtable.h"

//parses text to binary values
Hashtable* DtsodV24_deserialize(char* text);
//creates text representation of dtsod
char* DtsodV24_serialize(Hashtable* dtsod);
//returns value or UniNull if key not found
Unitype DtsodV24_get(Hashtable* dtsod, char* key);
//adds or sets value
void DtsodV24_addOrSet(Hashtable* dtsod, char* key, Unitype value);
//checks for dtsod contains value or dont
bool DtsodV24_contains(Hashtable* dtsod, char* key);
//replaces value with UniNull if key exists in dtsod
bool DtsodV24_remove(Hashtable* dtsod, char* key);

#if __cplusplus
}
#endif