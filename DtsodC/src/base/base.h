#pragma once

#include "std.h"
#include "types.h"
#include "errors.h"

// just sleeping function
// dont set 'milisec' > 1000 for good perfomance
void msleep(uint8 sec, uint16 milisec);
