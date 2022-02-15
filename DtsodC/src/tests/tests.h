#pragma once

#include "../base/base.h"

void printuni(Unitype v);

void test_autoarr(void);
void test_searchtree(void);
void test_all(void);
void test_autoarr2(void);

// executes codeblock and prints execution time
// should be used like optime({foo();}), because just optime(foo()) works slower
#define optime(codeblock) ({\
    clock_t start=clock();\
    (codeblock);\
    clock_t stop=clock();\
    printf("\e[96m%li\n",(stop-start));\
})
