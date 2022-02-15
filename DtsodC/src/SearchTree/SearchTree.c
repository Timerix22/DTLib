#include "SearchTree.h"

STNode* STNode_create(){
    STNode* node=malloc(sizeof(STNode));
    node->branches=NULL;
    node->value.type=Null;
    node->value.UInt64=0;
    return node;
}

void STNode_free(STNode* node){
    if (!node) throw(ERR_NULLPTR);
    if(node->branches){
        for(uint8 n32 = 0;n32<8;n32++){
            STNode*** ptrn32=(STNode***)node->branches[n32];
            if(ptrn32){ 
                for(uint8 n4 = 0;n4<8;n4++){
                    STNode** ptrn4=ptrn32[n4];
                    if (ptrn4){
                        for(uint8 rem=0;rem<4;rem++){
                            STNode* ptrrem=ptrn4[rem];
                            if(ptrrem)
                                STNode_free(ptrrem);
                        }
                        free(ptrn4);
                    }
                }
                free(ptrn32);
            }
        }
        free(node->branches);
    }
    if(node->value.VoidPtr) 
        switch (node->value.type) {   
            case Int8Ptr: case UInt8Ptr: 
            case Int16Ptr: case UInt16Ptr: 
            case Int32Ptr: case UInt32Ptr: 
            case Int64Ptr: case UInt64Ptr:
                free(node->value.VoidPtr);
                break;
            /*case AutoarrPtr:
                Autoarr_clear((Autoarr*)node->value.VoidPtr);
                free(node->value.VoidPtr);
                break;*/
            case STNodePtr:
                STNode_free((STNode*)node->value.VoidPtr);
                break;
            default: // value is not ptr
                break;
    }
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

void ST_push(STNode* node_first, const char* key, Unitype value){
    if (!node_first) throw(ERR_NULLPTR);
    STNode* node_last=node_first;
    while(*key){
        indexes3 i3=splitindex((uint8)*key);
        if(!node_last->branches){
            node_last->branches=(STNode****)malloc(8*sizeof(STNode*));
            for(uint8 i=0;i<8;i++)
                node_last->branches[i]=(STNode***)NULL;
        }
        if(!node_last->branches[i3.n32]){
            node_last->branches[i3.n32]=(STNode***)malloc(8*sizeof(STNode*));
            for(uint8 i=0;i<8;i++)
                node_last->branches[i3.n32][i]=(STNode**)NULL;
        }
        if(!node_last->branches[i3.n32][i3.n4]){
            node_last->branches[i3.n32][i3.n4]=(STNode**)malloc(4*sizeof(STNode*));
            for(uint8 i=0;i<4;i++)
                node_last->branches[i3.n32][i3.n4][i]=(STNode*)NULL;
        }
        if(!node_last->branches[i3.n32][i3.n4][i3.rem])
            node_last->branches[i3.n32][i3.n4][i3.rem]=STNode_create();
        node_last=node_last->branches[i3.n32][i3.n4][i3.rem];
        key++;
    }
    node_last->value=value;
}

const Unitype UnitypeNull={.type=Null,.VoidPtr=NULL};

Unitype ST_pull(STNode* node_first, const char* key){
    if (!node_first) throw(ERR_NULLPTR);
    STNode* node_last=node_first;
    while (*key){
        indexes3 i3=splitindex((uint8)*key);
        if(!node_last->branches) return UnitypeNull;
        STNode*** ptrn32=(STNode***)node_last->branches[i3.n32];
        if(!ptrn32) return UnitypeNull;
        STNode** ptrn4=ptrn32[i3.n4];
        if(!ptrn4) return UnitypeNull;
        node_last=ptrn4[i3.rem];
        if(!node_last) return UnitypeNull;
        key++;
    }
    return node_last->value;
}
