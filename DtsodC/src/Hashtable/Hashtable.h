#pragma once

#include "../base/base.h"
#include "../Autoarr/Autoarr.h"
#include "hash.h"

typedef struct Hashtable{
    uint16 height;    // amount of rows
    Autoarr** rows;    // Autoarr[height]s
} Hashtable;

Hashtable Hashtable_create(uint16 height,my_type type);

void Hashtable_clear(Hashtable* ht);

typedef struct KeyValuePair{
    char* key;
    Unitype value;
} KeyValuePair;

const uint16 Hashtable_HTINY=61;
const uint16 Hashtable_HSMALL=631;
const uint16 Hashtable_HMED=3889;
const uint16 Hashtable_HLARGE=19441;
const uint16 Hashtable_HMAX=65536;
