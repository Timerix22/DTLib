#pragma once

#include "../base/base.h"

typedef struct UniversalType{
    base_type type; 
    void* ptr;
} Unitype;

typedef struct SearchTreeNode{
    struct SearchTreeNode**** branches; //*STNode[8][8][4]
    Unitype value;
} STNode;

STNode* STNode_create(void);
void STNode_free(STNode* node);

void ST_push(STNode* node, const char* key, Unitype value);
Unitype ST_pull(STNode* node, const char* key);