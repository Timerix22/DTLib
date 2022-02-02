#pragma once

#include "../base/base.h"

typedef struct Autoarr{         // a collection with dynamic memory allocation
    base_type type;             // type of data
    uint16 max_block_count;     // max amount of blocks
    uint16 curr_block_count;    // amount of blocks
    uint16 max_block_length;    // max amount of data in block
    uint16 curr_block_length;   // amount of data in the last block
    uint32 max_length;          // max_block_count*max_block_length
    uint32 curr_length;         // (curr_block_count-1)*max_block_length+curr_block_length
    void** values;              // pointers to blocks
} Autoarr;

Autoarr Autoarr_create(uint16 _max_block_count, uint16 _max_block_length, base_type _type);
void Autoarr_clear(Autoarr* ar);

void Autoarr_add_int8(Autoarr *ar, int8 element);
void Autoarr_add_uint8(Autoarr *ar, uint8 element);
void Autoarr_add_int16(Autoarr *ar, int16 element);
void Autoarr_add_uint16(Autoarr *ar, uint16 element);
void Autoarr_add_int32(Autoarr *ar, int32 element);
void Autoarr_add_uint32(Autoarr *ar, uint32 element);
void Autoarr_add_int64(Autoarr *ar, int64 element);
void Autoarr_add_uint64(Autoarr *ar, uint64 element);

int8 Autoarr_get_int8(Autoarr *ar, uint32 index);
uint8 Autoarr_get_uint8(Autoarr *ar, uint32 index);
int16 Autoarr_get_int16(Autoarr *ar, uint32 index);
uint16 Autoarr_get_uint16(Autoarr *ar, uint32 index);
int32 Autoarr_get_int32(Autoarr *ar, uint32 index);
uint32 Autoarr_get_uint32(Autoarr *ar, uint32 index);
int64 Autoarr_get_int64(Autoarr *ar, uint32 index);
uint64 Autoarr_get_uint64(Autoarr *ar, uint32 index);

void Autoarr_set_int8(Autoarr *ar, uint32 index, int8 element);
void Autoarr_set_uint8(Autoarr *ar, uint32 index, uint8 element);
void Autoarr_set_int16(Autoarr *ar, uint32 index, int16 element);
void Autoarr_set_uint16(Autoarr *ar, uint32 index, uint16 element);
void Autoarr_set_int32(Autoarr *ar, uint32 index, int32 element);
void Autoarr_set_uint32(Autoarr *ar, uint32 index, uint32 element);
void Autoarr_set_int64(Autoarr *ar, uint32 index, int64 element);
void Autoarr_set_uint64(Autoarr *ar, uint32 index, uint64 element);
