#pragma once

#include "../base/base.h"
#include "../Autoarr/Autoarr.h"
#include "hash.h"

typedef struct Hashtable{
    my_type type;        // data type
    uint16 height;       // amount of rows
    Autoarr* rows;      // Autoarr[height]s
} Hashtable;

Hashtable Hashtable_create(uint16 height,my_type type);

void Hashtable_clear(Hashtable* ht);

void Hashtable_add_uni(Hashtable* ht,hash_t hash, Unitype val);
