#include "!headers.h"
#include "strict_types.h"

#ifndef __NSLEEP_DEFINED
#define __NSLEEP_DEFINED
void nsleep(uint8 sec, uint8 milisec){
    if (sec>0)
        sleep(sec);
    if (milisec>0)
        usleep(milisec*1000);
}
#endif