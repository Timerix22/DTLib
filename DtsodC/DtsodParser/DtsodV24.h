#pragma once
#include "../base/base.h"
#include "../Hashtable/Hashtable.h"

Hashtable* DtsodV24_deserialize(char* text);
char* DtsodV24_serialize(Hashtable* dtsod);
