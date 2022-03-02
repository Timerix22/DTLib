#include "../base/base.h"
#include "../Autoarr/Autoarr.h"

typedef struct KeyValuePair{
    char* key;
    Unitype value;
} KeyValuePair;

declare_Autoarr(KeyValuePair)

//proper way to clear a KVP
void KeyValuePair_free(KeyValuePair p);

//func to clear KVP array
void Autoarr_KeyValuePair_clear(Autoarr_KeyValuePair* ar);
