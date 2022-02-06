#include "base.h"

void msleep(uint8 sec, uint16 milisec){
    if (sec>0)
        sleep(sec);
    if (milisec>0)
        usleep(milisec*1000);
}
