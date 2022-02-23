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
    double t=(double)(stop-start)/CLOCKS_PER_SEC/repeats;\
    printf("\e[93moperation \e[94m%s\e[93m took \e[94m%lf \e[93mseconds\n",opname,t);\
})
