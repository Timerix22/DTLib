#include "../base/base.h"
#include "../Autoarr/Autoarr2.h"

typedef struct KeyValuePair{
    char* key;
    Unitype value;
} KeyValuePair;

declare_Autoarr2(KeyValuePair)

//proper way to clear a KVP
void KeyValuePair_free(KeyValuePair p);

//func to clear KVP array
void Autoarr2_KeyValuePair_clear(Autoarr2_KeyValuePair* ar);
