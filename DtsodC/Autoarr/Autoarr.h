#pragma once

#include "Autoarr_declare.h"
#include "Autoarr_define.h"

declare_Autoarr(uint8)
declare_Autoarr(int8)
declare_Autoarr(uint16)
declare_Autoarr(int16)
declare_Autoarr(uint32)
declare_Autoarr(int32)
declare_Autoarr(uint64)
declare_Autoarr(int64)
declare_Autoarr(float)
declare_Autoarr(double)
declare_Autoarr(Unitype)

//right func to clear array of unitype values
void Autoarr_Unitype_clear(Autoarr(Unitype)* ar);
