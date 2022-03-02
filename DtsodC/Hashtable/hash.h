#pragma once

#include "../base/base.h"

//djb2 hash function from http://www.cse.yorku.ca/~oz/hash.html
uint32 ihash(char *str);
//sdbm hash function
uint64 lhash(char* str);
