#pragma once

#include "types.h"

//returns length of string (including \0)
uint32 mystrlen(char* str);

//allocates new char[] and copies src there
char* mystrcpy(char* src);

//compares two strings, NullPtr-friendly
bool mystrcmp(char* key0, char* key1);

//multiplies char n times
char* mystrmtpl(char c, uint32 n);
