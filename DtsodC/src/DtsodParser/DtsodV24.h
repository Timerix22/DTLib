#pragma once
#include "../base/base.h"
#include "../Hashtable/Hashtable.h"

Hashtable* DtsodV24_deserialize(char* text);
string DtsodV24_serialize(Hashtable* dtsod);
