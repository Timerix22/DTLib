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

//my fixed length string struct
//doesn't store '\0' at the end
typedef struct string{
    char* ptr;      //char pointer
    uint32 length;  //amount of chars in ptr value
} string;

static const string stringNull={NULL,0};

//copies str content to new char pointer value (adding '\0' at the end)
char* string_toCharPtr(string str);

//copies cptr content (excluding '\0' at the end) to new string
string string_fromCharPtr(char* cptr);
