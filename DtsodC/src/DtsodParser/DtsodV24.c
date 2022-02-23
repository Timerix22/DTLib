#include "DtsodV24.h"


Hashtable* __parse(const char* text, bool called_recursively){
    Hashtable* dict=Hashtable_create();
    /* char c;

    char* ReadName(){

    };

    Unitype ReadValue(){

    };

    void SkipComment(){

    };


    while ((c=*text)){
        char* n=ReadName();
        Unitype v=ReadValue();
        Hashtable_add(dict,n,v);
        text++;
    } */
    
    return dict;
}

Hashtable* DtsodV24_parse(const char* text) { return __parse(text,false); }
