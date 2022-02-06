#pragma once

#include "../base/base.h"

// can store any base type
typedef struct UniversalType{
    base_type type; 
    union {
        int8 Int8;
        uint8 UInt8;
        int16 Int16;
        uint16 UInt16;
        int32 Int32;
        uint32 UInt32;
        int64 Int64;
        uint64 UInt64;
        float Float;
        double Double;
        void* VoidPtr;
    };
} Unitype;

typedef struct SearchTreeNode{
    struct SearchTreeNode**** branches; //*STNode[8][8][4]
    Unitype value;
} STNode;

STNode* STNode_create(void);
//doesn't work!
void STNode_free(STNode* node);

void ST_push(STNode* node, const char* key, Unitype value);
Unitype ST_pull(STNode* node, const char* key);
