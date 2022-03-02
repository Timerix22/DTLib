#pragma once

#include "Autoarr.h"

typedef Autoarr(int8) StringBuilder;

StringBuilder StringBuilder_create(uint16 max_blocks_count, uint16 max_block_length);
void StringBuilder_append(StringBuilder* b, char c);
void StringBuilder_append_str(StringBuilder* b, char* s);
char* StringBuilder_build(StringBuilder* b);
