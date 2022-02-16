#pragma once

#include "../base/base.h"

#define define_Autoarr2(type)\
\
void __Autoarr2_add_##type(Autoarr2_##type* ar, type element){\
    if(!ar->values){\
        ar->values=malloc(ar->max_blocks_count*sizeof(type*));\
        goto create_block;\
    }\
    if(ar->block_length==ar->max_block_length){\
        if (ar->blocks_count>=ar->max_blocks_count) throw(ERR_MAXLENGTH);\
        ar->block_length=0;\
create_block:\
        ar->values[ar->blocks_count]=malloc(ar->max_block_length*sizeof(type));\
        ar->blocks_count++;\
    }\
    ar->values[ar->blocks_count-1][ar->block_length]=element;\
    ar->block_length++;\
}\
\
type __Autoarr2_get_##type(Autoarr2_##type* ar, uint32 index){\
    if(index>=Autoarr2_length(ar)) throw(ERR_WRONGINDEX);\
    return ar->values[index/ar->max_block_length][index%ar->max_block_length];\
}\
\
type* __Autoarr2_getptr_##type(Autoarr2_##type* ar, uint32 index){\
    if(index>=Autoarr2_length(ar)) throw(ERR_WRONGINDEX);\
    return ar->values[index/ar->max_block_length]+(index%ar->max_block_length);\
}\
\
void __Autoarr2_set_##type(Autoarr2_##type* ar, uint32 index, type element){\
    if(index>=Autoarr2_length(ar)) throw(ERR_WRONGINDEX);\
    ar->values[index/ar->max_block_length][index%ar->max_block_length]=element;\
}\
\
void __Autoarr2_clear_##type(Autoarr2_##type* ar){\
    for(uint16 i=0; i<ar->blocks_count;i++)\
                free(ar->values[i]); \
    free(ar->values);\
    ar->values=NULL;\
    ar->blocks_count=0;\
    ar->block_length=0;\
}\
\
__functions_list_t_##type __functions_list_##type={\
    &__Autoarr2_add_##type,\
    &__Autoarr2_get_##type,\
    &__Autoarr2_getptr_##type,\
    &__Autoarr2_set_##type,\
    &__Autoarr2_clear_##type\
};\
\
Autoarr2_##type __Autoarr2_create_##type(uint16 max_blocks_count, uint16 max_block_length){\
    return (Autoarr2_##type){\
        .max_blocks_count=max_blocks_count,\
        .blocks_count=0,\
        .max_block_length=max_block_length,\
        .block_length=0,\
        .values=NULL,\
        .functions=&__functions_list_##type\
    };\
}
