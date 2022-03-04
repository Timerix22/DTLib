#pragma once

#include "../base/base.h"

#define declare_Autoarr(type)\
\
struct Autoarr_##type;\
\
typedef struct {\
    void (*add)(struct Autoarr_##type* ar, type element);\
    type (*get)(struct Autoarr_##type* ar, uint32 index);\
    type* (*getptr)(struct Autoarr_##type* ar, uint32 index);\
    void (*set)(struct Autoarr_##type* ar, uint32 index, type element);\
    void (*clear)(struct Autoarr_##type* ar);\
} __functions_list_t_##type;\
\
typedef struct Autoarr_##type{\
    uint16 blocks_count;\
    uint16 max_blocks_count;\
    uint16 block_length;\
    uint16 max_block_length;\
    type** values;\
    __functions_list_t_##type* functions;\
} Autoarr_##type;\
\
void __Autoarr_add_##type(Autoarr_##type* ar, type element);\
type __Autoarr_get_##type(Autoarr_##type* ar, uint32 index);\
type* __Autoarr_getptr_##type(Autoarr_##type* ar, uint32 index);\
void __Autoarr_set_##type(Autoarr_##type* ar, uint32 index, type element);\
void __Autoarr_clear_##type(Autoarr_##type* ar);\
Autoarr_##type __Autoarr_create_##type(uint16 max_blocks_count, uint16 max_block_length);

#define Autoarr(type) Autoarr_##type

#define Autoarr_add(autoarr, element)\
    autoarr->functions->add(autoarr, element)
#define Autoarr_get(autoarr, index)\
    autoarr->functions->get(autoarr,index)
#define Autoarr_getptr(autoarr, index)\
    autoarr->functions->getptr(autoarr,index)
#define Autoarr_set(autoarr, index, element)\
    autoarr->functions->set(autoarr, index, element)
#define Autoarr_clear(autoarr)\
    autoarr->functions->clear(autoarr)
#define Autoarr_create(type, max_blocks_count, max_block_length)\
    __Autoarr_create_##type(max_blocks_count, max_block_length)

#define Autoarr_length(autoarr) \
    (uint32)(!autoarr->blocks_count ? 0 : \
    autoarr->max_block_length*(autoarr->blocks_count-1)+autoarr->block_length)
#define Autoarr_max_length(autoarr)\
    (uint32)(autoarr->max_block_length*autoarr->max_blocks_count)
    
#define Autoarr_remove(AR){\
    if(AR->block_length==1){\
                        AR->blocks_count--;\
                         AR->block_length=AR->max_block_length;\
                         free(AR->values[AR->blocks_count]);\
                    }\
                    else AR->block_length--;\
}
