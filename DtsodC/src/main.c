#include "base/base.h"
#include "tests/tests.h"
#include "Autoarr/Autoarr.h"
#include "SearchTree/SearchTree.h"

#define clrscr() printf("\e[1;1H\e[2J")

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    test_all();
    Unitype a={Null,NULL};
    STNode* node=STNode_create();
    ST_push(node,"aboba", a);
    return 0;
}
