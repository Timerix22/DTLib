#include "tests.h"
#include "../Hashtable/Hashtable.h"

void print_hashtable(Hashtable* ht){
    printf("\e[94mHashtable:%lu\n"
        "  hein: %u\n"
        "  height: %u\n"
        "  rows: %p\n",
        sizeof(Hashtable),
        ht->hein,
        Hashtable_height(ht),
        ht->rows);
}

void hashtable_fill(Hashtable* ht){
    char* key=malloc(20);
    for(uint32 i=0;i<255;i++){
        sprintf(key,"key__%u",i);
        Hashtable_add(ht,key,Uni(UInt32,i));
    }
    free(key);
}

void hashtable_printval(Hashtable* ht){
    char* key=malloc(20);
    for(uint32 i=0;i<255;i++){
        sprintf(key,"key__%u",i);
        printuni(Hashtable_get(ht,key));
        printf("  ");
    }
    free(key);
}


void test_hashtable(void){
    optime("test_hashtable",1,({
        printf("\e[96m-----------[test_hashtable]------------\n");
        Hashtable* ht=Hashtable_create();
        print_hashtable(ht);
        hashtable_fill(ht);
        printf("\e[92mhashtable filled\n\e[90m");
        hashtable_printval(ht);
        Hashtable_free(ht);
        printf("\n\e[92mhashtable freed\n");
    }));
}
