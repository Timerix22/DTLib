#include "SearchTree.h"
    #include "../tests/tests.h"

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
    //if value is not freed ptr
    if(node->value.type>12 && node->value.type<21 && node->value.VoidPtr) 
        free(node->value.VoidPtr);
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

//int16 globytes=0;
void ST_push(STNode* node_first, const char* key, Unitype value){
    if (!node_first) throw(ERR_NULLPTR);
    //int16 bytes=sizeof(Unitype);
    char c = *key;
    STNode* node_last=node_first;
    for (uint16 i=0;c!='\0';){
        indexes3 i3=splitindex((uint8)c);
        if(!node_last->branches){
            node_last->branches=(STNode****)malloc(8*sizeof(STNode*));
            //bytes+=sizeof(void*)*8;
            for(uint8 i=0;i<8;i++)
                node_last->branches[i]=(STNode***)NULL;
        }
        STNode*** ptrn32=(STNode***)node_last->branches[i3.n32];
        if(!ptrn32){
            ptrn32=(STNode***)malloc(8*sizeof(STNode*));
            //bytes+=sizeof(void*)*8;
            for(uint8 i=0;i<8;i++)
                ptrn32[i]=(STNode**)NULL;
            node_last->branches[i3.n32]=ptrn32;
        }
        STNode** ptrn4=ptrn32[i3.n4];
        if(!ptrn4){
            ptrn4=(STNode**)malloc(4*sizeof(STNode*));
            //bytes+=sizeof(void*)*4;
            for(uint8 i=0;i<4;i++)
                ptrn4[i]=(STNode*)NULL;
            ptrn32[i3.n4]=ptrn4;
        }
        node_last=ptrn4[i3.rem];
        if(!node_last){
            node_last=STNode_create();
            //bytes+=sizeof(STNode);
            ptrn4[i3.rem]=node_last;
        }
        c=*(key+(++i));
    }
    node_last->value=value;
    //globytes+=bytes;
    //dbg(globytes);
}

const Unitype UnitypeNull={Null,.VoidPtr=NULL};

Unitype ST_pull(STNode* node_first, const char* key){
    if (!node_first) throw(ERR_NULLPTR);
    char c = *key;
    STNode* node_last=node_first;
    for (uint16 i=0;c!='\0';){
        indexes3 i3=splitindex((uint8)c);
        if(!node_last->branches) return UnitypeNull;
        STNode*** ptrn32=(STNode***)node_last->branches[i3.n32];
        if(!ptrn32) return UnitypeNull;
        STNode** ptrn4=ptrn32[i3.n4];
        if(!ptrn4) return UnitypeNull;
        node_last=ptrn4[i3.rem];
        if(!node_last) return UnitypeNull;
        c=*(key+(++i));
    }
    return node_last->value;
}
