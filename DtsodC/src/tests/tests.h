#pragma once

#include "../base/base.h"

void printuni(Unitype v);

void test_all(void);
void test_searchtree(void);
void test_autoarr2(void);
void test_hashtable(void);

// executes codeblock and prints execution time
// should be used like optime({foo();}), because just optime(foo()) works slower
#define optime(opname,repeats,codeblock) ({\
    clock_t start=clock();\
    for(uint64 ___OPREP=0;___OPREP<repeats;___OPREP++)\
        (codeblock);\
    clock_t stop=clock();\
    printf("\e[93moperation %s took \e[94m%ld \e[93mticks\n",opname,(stop-start));\
})
