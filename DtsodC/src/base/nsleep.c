#pragma once

#include "std.h"
#include "base_types.h"

void nsleep(uint8 sec, uint8 milisec){
    if (sec>0)
        sleep(sec);
    if (milisec>0)
        usleep(milisec*1000);
}
