#include "Autoarr.h"

Autoarr Autoarr_create(uint16 _max_block_count, uint16 _max_block_length, base_type _type){
    Autoarr ar={
        .type=_type,
        .max_block_count=_max_block_count,
        .max_block_length=_max_block_length,
        .curr_block_count=1,
        .curr_block_length=0,
        .max_length=_max_block_count*_max_block_length,
        .curr_length=0,
        .values=malloc(_max_block_count*typesize(_type))
    };
    *ar.values=malloc(ar.max_block_length*typesize(ar.type));
    return ar;
}

// creates new block if the current one is filled
void __Autoarr_create_block(Autoarr *ar){
    if (ar->curr_block_count>=ar->max_block_count) throw(ERR_MAXLENGTH);
    //if (ar->curr_block_length==ar->max_block_length) 
        ar->curr_block_length=0;
    //else throw("current block isn't filled");
    *(ar->values+ar->curr_block_count)=malloc(ar->max_block_length*typesize(ar->type));
    ar->curr_block_count++;
}

void __Autoarr_add_pre(Autoarr* ar, base_type t){
    if(ar->type!=t) throw(ERR_WRONGTYPE);
    if(ar->curr_length>=ar->max_length)  throw(ERR_MAXLENGTH);
    if (ar->curr_block_length==ar->max_block_length)
        __Autoarr_create_block(ar);
    ar->curr_block_length++;
    ar->curr_length++;
}

void Autoarr_add_int8(Autoarr *ar, int8 element){
    __Autoarr_add_pre(ar,Int8);
    *(*((int8**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_uint8(Autoarr *ar, uint8 element){
    __Autoarr_add_pre(ar,UInt8);
    *(*((uint8**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_int16(Autoarr *ar, int16 element){
    __Autoarr_add_pre(ar,Int16);
    *(*((int16**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_uint16(Autoarr *ar, uint16 element){
    __Autoarr_add_pre(ar,UInt16);
    *(*((uint16**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_int32(Autoarr *ar, int32 element){
    __Autoarr_add_pre(ar,Int32);
    *(*((int32**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_uint32(Autoarr *ar, uint32 element){
    __Autoarr_add_pre(ar,UInt32);
    *(*((uint32**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_int64(Autoarr *ar, int64 element){
    __Autoarr_add_pre(ar,Int64);
    *(*((int64**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}
void Autoarr_add_uint64(Autoarr *ar, uint64 element){
    __Autoarr_add_pre(ar,UInt64);
    *(*((uint64**)ar->values+ar->curr_block_count-1)+ar->curr_block_length-1)=element;
}

// calculates a block number and element position in the block
// also verifies type of array
div_t __Autoarr_div_index(Autoarr* ar, uint32 i, base_type t){
    if(ar->type!=t) throw(ERR_WRONGTYPE);
    if(i>=ar->curr_length) throw(ERR_WRONGINDEX);
    return (div_t){
        .quot=i/ar->max_block_length,
        .rem=i%ar->max_block_length
    };;
}

int8 Autoarr_get_int8(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, Int8);
    return *(*((int8**)ar->values+d.quot)+d.rem);
}
uint8 Autoarr_get_uint8(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, UInt8);
    return *(*((uint8**)ar->values+d.quot)+d.rem);
}
int16 Autoarr_get_int16(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, Int16);
    return *(*((int16**)ar->values+d.quot)+d.rem);
}
uint16 Autoarr_get_uint16(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, UInt16);
    return *(*((uint16**)ar->values+d.quot)+d.rem);
}
int32 Autoarr_get_int32(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, Int32);
    return *(*((int32**)ar->values+d.quot)+d.rem);
}
uint32 Autoarr_get_uint32(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, UInt32);
    return *(*((uint32**)ar->values+d.quot)+d.rem);
}
int64 Autoarr_get_int64(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, Int64);
    return *(*((int64**)ar->values+d.quot)+d.rem);
}
uint64 Autoarr_get_uint64(Autoarr *ar, uint32 index){
    div_t d = __Autoarr_div_index(ar, index, UInt64);
    return *(*((uint64**)ar->values+d.quot)+d.rem);
}

void Autoarr_set_int8(Autoarr *ar, uint32 index, int8 element){
    div_t d =__Autoarr_div_index(ar, index, Int8);
    *(*((int8**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_uint8(Autoarr *ar, uint32 index, uint8 element){
    div_t d =__Autoarr_div_index(ar, index, UInt8);
    *(*((uint8**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_int16(Autoarr *ar, uint32 index, int16 element){
    div_t d =__Autoarr_div_index(ar, index, Int16);
    *(*((int16**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_uint16(Autoarr *ar, uint32 index, uint16 element){
    div_t d =__Autoarr_div_index(ar, index, UInt16);
    *(*((uint16**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_int32(Autoarr *ar, uint32 index, int32 element){
    div_t d =__Autoarr_div_index(ar, index, Int32);
    *(*((int32**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_uint32(Autoarr *ar, uint32 index, uint32 element){
    div_t d =__Autoarr_div_index(ar, index, UInt32);
    *(*((uint32**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_int64(Autoarr *ar, uint32 index, int64 element){
    div_t d =__Autoarr_div_index(ar, index, Int64);
    *(*((int64**)ar->values+d.quot)+d.rem)=element;
}
void Autoarr_set_uint64(Autoarr *ar, uint32 index, uint64 element){
    div_t d =__Autoarr_div_index(ar, index, UInt64);
    *(*((uint64**)ar->values+d.quot)+d.rem)=element;
}

void Autoarr_free(Autoarr* ar){
    switch (ar->type) {   
        case Int8:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((int8**)ar->values+i)); break;
        case UInt8:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((uint8**)ar->values+i)); break;
        case Int16:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((int16**)ar->values+i)); break;
        case UInt16:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((uint16**)ar->values+i)); break;
        case Int32:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((int32**)ar->values+i)); break;
        case UInt32:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((uint32**)ar->values+i)); break;
        case Int64:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((int64**)ar->values+i)); break;
        case UInt64:
            for(uint16 i = 0; i < ar->curr_block_count-1;i++)
                free(*((uint64**)ar->values+i)); break;
        default: throw(ERR_WRONGTYPE); break;
    }
    free(ar->values);
    ar->type=0;
    ar->max_block_count=0;
    ar->max_block_length=0;
    ar->curr_block_count=0;
    ar->curr_block_length=0;
    ar->max_length=0;
    ar->curr_length=0;
}
