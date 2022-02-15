#pragma once

#include "../base/base.h"

#define declare_Autoarr2(type)\
\
struct Autoarr2_##type;\
\
typedef struct {\
    void (*add)(struct Autoarr2_##type* ar, type element);\
    type (*get)(struct Autoarr2_##type* ar, uint32 index);\
    void (*set)(struct Autoarr2_##type* ar, uint32 index, type element);\
    void (*clear)(struct Autoarr2_##type* ar);\
} __functions_list_t_##type;\
\
typedef struct Autoarr2_##type{\
    uint16 blocks_count;\
    uint16 max_blocks_count;\
    uint16 block_length;\
    uint16 max_block_length;\
    type** values;\
    __functions_list_t_##type* functions;\
} Autoarr2_##type;\
\
void __Autoarr2_add_##type(Autoarr2_##type* ar, type element);\
type __Autoarr2_get_##type(Autoarr2_##type* ar, uint32 index);\
void __Autoarr2_set_##type(Autoarr2_##type* ar, uint32 index, type element);\
void __Autoarr2_clear_##type(Autoarr2_##type* ar);\
Autoarr2_##type __Autoarr2_create_##type(uint16 max_blocks_count, uint16 max_block_length);

#define Autoarr2(type) Autoarr2_##type

#define Autoarr2_add(autoarr, element)\
    autoarr->functions->add(autoarr, element)
#define Autoarr2_get(autoarr, index)\
    autoarr->functions->get(autoarr,index)
#define Autoarr2_set(autoarr, index, element)\
    autoarr->functions->set(autoarr, index, element)
#define Autoarr2_clear(autoarr)\
    autoarr->functions->clear(autoarr)
#define Autoarr2_create(type, max_blocks_count, max_block_length)\
    __Autoarr2_create_##type(max_blocks_count, max_block_length)

#define Autoarr2_length(autoarr) \
    (uint32)(!autoarr->blocks_count ? 0 : \
    autoarr->max_block_length*(autoarr->blocks_count-1)+autoarr->block_length)
#define Autoarr2_max_length(autoarr)\
    (uint32)(autoarr->max_block_length*autoarr->max_blocks_count)
