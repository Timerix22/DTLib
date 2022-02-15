#include "base.h"

void fsleep(float sec){
    struct timespec t;
    t.tv_sec=sec+0.0001f;
    t.tv_nsec=(sec-t.tv_sec)*1000000000;
    nanosleep(&t, NULL);
}
