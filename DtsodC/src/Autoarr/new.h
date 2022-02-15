#include "../base/base.h"

#ifndef AUTOARR_H
#define AUTOARR_H

#define Autoarr_length(autoarr) (uint32)(autoarr->max_block_length*(autoarr->max_blocks_count-1)+autoarr->block_length)
#define Autoarr_max_length(autoarr) (uint32)(autoarr->max_block_length*autoarr->max_blocks_count)

#define define_Autoarr(type) \
typedef struct Autoarr_##type{ \
    uint16 blocks_count; \
    uint16 max_blocks_count; \
    uint16 block_length; \
    uint16 max_block_length; \
    type** values; \
    void (*Autoarr_add)(struct Autoarr_##type* ar, type element); \
    type (*Autoarr_get)(struct Autoarr_##type* ar, uint32 index); \
    void (*Autoarr_set)(struct Autoarr_##type* ar, uint32 index, type element); \
} Autoarr_##type; \
\
void Autoarr_add_##type(Autoarr_##type* ar, type element){ \
    if(!ar->values){ \
        ar->values=malloc(ar->max_blocks_count*sizeof(type*)); \
        goto create_block; \
    } \
    if(ar->block_length==ar->max_block_length){ \
        if (ar->blocks_count>=ar->max_blocks_count) throw(ERR_MAXLENGTH); \
        ar->block_length=0; \
create_block: \
        ar->values[ar->blocks_count]=malloc(ar->max_block_length*sizeof(type)); \
        ar->blocks_count++; \
    } \
    ar->values[ar->blocks_count-1][ar->block_length]=element; \
    ar->block_length++; \
} \
\
type Autoarr_get_##type(Autoarr_##type* ar, uint32 index){ \
    if(index>=Autoarr_length(ar)) throw(ERR_WRONGINDEX); \
    return ar->values[index/ar->max_block_length][index%ar->max_block_length]; \
} \
\
void Autoarr_set_##type(Autoarr_##type* ar, uint32 index, type element){ \
    if(index>=Autoarr_length(ar)) throw(ERR_WRONGINDEX); \
    ar->values[index/ar->max_block_length][index%ar->max_block_length]=element; \
} \
\
Autoarr_##type Autoarr_create_##type(uint16 max_blocks_count, uint16 max_block_length){ \
    return (Autoarr_##type){ \
        .max_blocks_count=max_blocks_count, \
        .blocks_count=0, \
        .max_block_length=max_block_length, \
        .block_length=0, \
        .values=NULL, \
        .Autoarr_add=Autoarr_add_##type, \
        .Autoarr_get=Autoarr_get_##type, \
        .Autoarr_set=Autoarr_set_##type \
    }; \
} \
\
void Autoarr_clear_##type(Autoarr_##type* ar){ \
    for(uint16 i=0; i<ar->blocks_count;i++) \
                free(ar->values[i]);  \
    free(ar->values); \
    ar->values=NULL; \
    ar->blocks_count=0; \
    ar->block_length=0; \
}

#define Autoarr(type) Autoarr_##type
#define Autoarr_create(type, max_blocks_count, max_block_length) \
    Autoarr_create_##type(max_blocks_count, max_block_length)
    
#endif
