#pragma once

#include "std.h"
#include "types.h"
#include "errors.h"
#include "mystr.h"

// sleep function based on std nanosleep()
void fsleep(float sec);

// executes codeblock and prints execution time
#define optime(opname,repeats,codeblock) ({\
    clock_t start=clock();\
    for(uint64 ___OPREP=0;___OPREP<repeats;___OPREP++)\
        (codeblock);\
    clock_t stop=clock();\
    double t=(double)(stop-start)/CLOCKS_PER_SEC/repeats;\
    printf("\e[93moperation \e[94m%s\e[93m took \e[94m%lf \e[93mseconds\n",opname,t);\
})
