#pragma once

#include "../base/base.h"
#include "hash.h"
#include "KeyValuePair.h"

typedef struct Hashtable{
    uint8 hein;  //height=HT_HEIGHTS[hein] 
    Autoarr(KeyValuePair)* rows; // Autoarr[height]
} Hashtable;

Hashtable* Hashtable_create();
void Hashtable_free(Hashtable* ht);

//amount of rows
uint32 Hashtable_height(Hashtable* ht);

//adds charptr and value to new KeyValuePair
//use charbuf_copy() to create new string if needed
#define KVPair(key,value) (KeyValuePair){key,value}


void Hashtable_add_pair(Hashtable* ht, KeyValuePair p);
void Hashtable_add(Hashtable* ht, char* key, Unitype u);

//returns null or pointer to value in hashtable
Unitype* Hashtable_getptr(Hashtable* ht, char* key);

Unitype Hashtable_get(Hashtable* ht, char* key);
KeyValuePair Hashtable_get_pair(Hashtable* ht, char* key);
bool Hashtable_try_get(Hashtable* ht, char* key, Unitype* output);

//not implemented yet
void Hashtable_set_pair(Hashtable* ht, KeyValuePair p);
void Hashtable_set(Hashtable* ht, char* key, Unitype u);
