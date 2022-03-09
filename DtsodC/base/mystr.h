#pragma once

#include "types.h"

//returns length of string (including \0)
uint32 cptr_length(char* str);

//allocates new char[] and copies src there
char* cptr_copy(char* src);

//compares two char buffers, NullPtr-friendly
bool cptr_compare(char* key0, char* key1);

//multiplies char n times
char* char_multiply(char c, uint32 n);

//my fixed length string struct
//doesn't store '\0' at the end
typedef struct string{
    char* ptr;      //char pointer
    uint32 length;  //amount of chars in ptr value
} string;

static const string stringNull={NULL,0};

//copies str content to new char pointer value (adding '\0' at the end)
char* string_cpToCharPtr(string str);

//copies cptr content (excluding '\0' at the end) to new string
string string_cpFromCharPtr(char* cptr);

//compares two strings, NullPtr-friendly
bool string_compare(string str0, string str1);

//creates new string which is reversed variant of <s>
string string_reverse(string s);
