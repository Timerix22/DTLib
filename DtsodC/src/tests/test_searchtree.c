#include "tests.h"
#include "../SearchTree/SearchTree.h"

void printn(STNode* node){
    printf("\e[94m  STNode: %lu\n"            
        "    branches: %p\n"
        "    value.type: %s\n"
        "    value.ptr: %p\n",
        sizeof(STNode),
        node->branches,
        typename(node->value.type),
        node->value.ptr
        );
}

void test_searchtree(){
    printf("\e[96m-----------[test_searchtree]-----------\n");
    STNode* node=STNode_create();
    printf("\e[92m  node created\n");
    printn(node);
    STNode_free(node);
    printf("\e[92m  node deleted\n");
}
