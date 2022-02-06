#include "tests.h"
#include "../SearchTree/SearchTree.h"

void printuni(Unitype v){
    switch (v.type) {
        case Null: printf("{%s}",typename(v.type));break;
        case Double: printf("{%s:%lf}",typename(v.type),v.Double);break;
        case Float: printf("{%s:%f}",typename(v.type),v.Float);break;
        case Char: printf("{%s:%c}",typename(v.type),v.Int8);break;
        case UInt8: 
        case UInt16: printf("{%s:%u}",typename(v.type),v.UInt16);break;
        case UInt32: 
        case UInt64: printf("{%s:%lu}",typename(v.type),v.UInt64);break;
        case Bool: 
        case Int8: 
        case Int16: printf("{%s:%d}",typename(v.type),v.Int16);break;
        case Int32: 
        case Int64: printf("{%s:%ld}",typename(v.type),v.Int64);break;
        case Int8Ptr: 
        case UInt8Ptr: 
        case Int16Ptr: 
        case UInt16Ptr: 
        case Int32Ptr: 
        case UInt32Ptr: 
        case Int64Ptr: 
        case UInt64Ptr: printf("{%s:%p}",typename(v.type),v.VoidPtr);break;
        default: throw(ERR_WRONGTYPE);break;
    }
}

void printstnode(STNode* node){
    printf("\e[94mSTNode: %lu\n  address: %p\n  value: ",sizeof(STNode),node);
    printuni(node->value);
    // prints pointers to all existing branches
    printf("\n  branches: %p\n", node->branches);
    if(node->branches) for(uint8 i=0;i<8;i++){
        printf("    \e[90m[%u]=%p\n",i,node->branches[i]);
        for (uint8 ii = 0; ii < 8; ii++){
                if(node->branches[i]){
                    printf("      \e[90m[%u][%u]=%p\n",i,ii,node->branches[i][ii]);
                    for (uint8 iii = 0; iii < 4; iii++)
                        if(node->branches[i][ii]) printf("        \e[90m[%u][%u][%u]=%p\n",i,ii,iii,node->branches[i][ii][iii]);
                }
        }
    }
}

void test_searchtree(){
    printf("\e[96m-----------[test_searchtree]-----------\n");
    STNode* node=STNode_create();
    printf("\e[92mnode created\n");
    printstnode(node);
    STNode_free(node);
    printf("\e[92mnode deleted\n");
}
