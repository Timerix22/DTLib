#include "SearchTree.h"

STNode* STNode_create(){
    STNode* node=malloc(sizeof(STNode));
    node->branches=NULL;
    node->value.ptr=NULL;
    node->value.type=Null;
    return node;
}


void STNode_free(STNode* node){
    ifNthrow(node);
    if(node->branches!=NULL){
        for(uint8 n32 = 0;n32<8;n32++){
            STNode*** ptrn32=(STNode***)node->branches[n32];
            if(ptrn32!=NULL){ 
                for(uint8 n4 = 0;n4<8;n4++){
                    STNode** ptrn4=ptrn32[n4];
                    if (ptrn4!=NULL){
                        for(uint8 rem=0;rem<4;rem++){
                            STNode* ptrrem=ptrn4[rem];
                            if(ptrrem!=NULL){
                                STNode_free(ptrrem);
                                free(ptrrem);
                            }
                        }
                        free(ptrn4);
                    }
                }
                free(ptrn32);
            }
        }
        free(node->branches);
    }
    free(node->value.ptr);
    free(node);
}

typedef struct {uint8 n32, n4, rem;} indexes3;

indexes3 splitindex(uint8 i){
    return (indexes3){
        .n32=i/32,
        .n4=i%32/4,
        .rem=i%32%4,
    };
}

uint8 combinei3(indexes3 i3){
    return i3.n32*32+i3.n4*8;
}


// returns NULL or *STNode corresponding to the character
STNode* getcnode(STNode* node, uint8 c){
    indexes3 i3=splitindex(c);
    ifNretN(node->branches);
    STNode*** ptrn32=(STNode***)node->branches[i3.n32];
    ifNretN(ptrn32);
    STNode** ptrn4=ptrn32[i3.n4];
    ifNretN(ptrn4);
    return ptrn4[i3.rem];
}

void ST_push(STNode* node, const char* key, Unitype value){
    char c = *key;
    for (uint16 i=0;c!='\0';){
        printf("[%u]%c ",i,c);
        c=*(key+(++i));
    }
}
