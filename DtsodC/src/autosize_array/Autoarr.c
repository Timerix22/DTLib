#ifndef STDLIB
    #include "../../cosmopolitan/cosmopolitan.h"
#else
    #include <stdio.h>
    #include <stdlib.h>
    #include <math.h>
#endif
#include "Autoarr.h"


Autoarr* Autoarr_create(uint16 max_block_count, uint16 max_block_length, strict_type type){
    Autoarr* rezult=malloc(sizeof(Autoarr));
    rezult->type=type;
    rezult->max_block_length=max_block_length;
    rezult->curr_block_length=0;
    rezult->max_block_count=max_block_count;
    rezult->block_count=0;
    rezult->values=malloc(rezult->max_block_count*GetTypeSize(type));
    return rezult;
}

uint32 Autoarr_length(Autoarr *a){
    return (uint32)((a->block_count-1)*a->max_block_length+a->curr_block_length);
}
uint32 Autoarr_max_length(Autoarr *a){
    return a->max_block_count*a->max_block_length;
}

void __Autoarr_add_block(Autoarr *autoarr){
    if (autoarr->block_count>=autoarr->max_block_count)
        throwerr(ERR_MAXLENGTH);
    if (autoarr->curr_block_length==(autoarr->max_block_length|0))
        autoarr->curr_block_length=0;
    else if (autoarr->curr_block_length>0)
        throwerr(ERR_NOTFULL);
    *(autoarr->values+autoarr->block_count)=malloc(autoarr->max_block_length*GetTypeSize(autoarr->type));
    autoarr->block_count++;
}

void Autoarr_add(Autoarr *autoarr, void *element){
    if(Autoarr_length(autoarr)<Autoarr_max_length(autoarr))
        throwerr(ERR_MAXLENGTH);
    if (autoarr->curr_block_length==autoarr->max_block_length)
        __Autoarr_add_block(autoarr);
    AssignVoidToVoid((autoarr->values+autoarr->block_count-1)+autoarr->curr_block_length, element, autoarr->type);
}

void Autoarr_set(Autoarr *autoarr, uint32 index, void *element){
    
    
}

void Autoarr_remove(Autoarr *autoarr, uint32 index){

    
}

void Autoarr_removeRange(Autoarr *autoarr, uint32 startIndex, uint32 length){
    
}
