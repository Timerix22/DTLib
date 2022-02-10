#pragma once

#include "../base/base.h"

typedef uint32 hash_t;

//djb2 hash function from http://www.cse.yorku.ca/~oz/hash.html
hash_t dhash(char *str);
