#pragma once

#include "std.h"
#include "base_types.h"
#include "errors.h"

// just sleeping function
// dont set 'milisec' > 1000 for good perfomance
void msleep(uint8 sec, uint16 milisec);

//djb2 hash function from http://www.cse.yorku.ca/~oz/hash.html
uint32 hash(char *str);