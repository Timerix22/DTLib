#include "tests.h"
#include "../Hashtable/Hashtable.h"

void printkvp(KeyValuePair p){
    printf("{\"%s\", ",p.key);
    printuni(p.value);
    printf("}");
}

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

void printrowgraph(Hashtable* ht){
    printf("\e[94mrow length graph:\n");
    uint16 lgs_l=1000;
    uint32 lgs[lgs_l];
     for(uint32 i=0; i<lgs_l; i++)
        lgs[i]=0; 
    for(uint16 h=0;h<Hashtable_height(ht);h++){
        Autoarr(KeyValuePair)* ar=ht->rows+h;
        uint32 l=Autoarr_length(ar);
        lgs[l]++;
    }
    for(uint32 i=0; i<lgs_l; i++)
        if(lgs[i]>0) {
            char* str0=char_multiply(' ',i>=100?0:(i>=10?1:2));
            char* str1=char_multiply(' ',lgs[i]>=100?0:(lgs[i]>=10?1:2));
            char* str2=char_multiply('#',lgs[i]/100);
            printf("\e[94m  length: \e[96m%u %s \e[94mfrequency: \e[96m%u %s \e[90m%s\n",i,str0,lgs[i],str1,str2);
            free(str0);
            free(str1);
            free(str2);
        }
}

void fill(Hashtable* ht){
    for(uint32 i=0;i<100000;i++){
        char* key=malloc(12);
        sprintf(key,"key__%u",i);
        Hashtable_add(ht,key,Uni(UInt64,i));
    }
}

Unitype gett(Hashtable* ht){
    char* key=malloc(12);
    Unitype u;
    for(uint32 i=0;i<100000;i++){
        sprintf(key,"key__%u",i);
        u=Hashtable_get(ht,key);
    }
    free(key);
    return u;
}


void test_hashtable(){
    optime("test_hashtable",1,({
        printf("\e[96m-----------[test_hashtable]------------\n");
        Hashtable* ht=Hashtable_create();
        printf("\e[92mhashtable created\n");
        print_hashtable(ht);
        optime("fill",1,fill(ht));
        optime("get",1,gett(ht));
        printrowgraph(ht);
        print_hashtable(ht);
        Hashtable_free(ht);
        printf("\e[92mhashtable freed\n");
    }));
}
