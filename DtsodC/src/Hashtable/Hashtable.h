#pragma once

#include "../base/base.h"
#include "../Autoarr/Autoarr2.h"
#include "hash.h"

typedef struct KeyValuePair{
    char* key;
    Unitype value;
} KeyValuePair;

declare_Autoarr2(KeyValuePair)

typedef struct Hashtable{
    uint8 hein;  //height=HT_HEIGHTS[hein] 
    Autoarr2(KeyValuePair)* rows; // Autoarr[height]
} Hashtable;

// amount of rows
const uint16 HT_HEIGHTS[]={61,631,3889,19441,65521};
