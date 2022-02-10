#include "base/base.h"

#include "tests/tests.h"

#include "Autoarr/Autoarr.h"

#include "SearchTree/SearchTree.h"
#include "Hashtable/hash.c"



#define clrscr() printf("\e[1;1H\e[2J")



int main(){

    setlocale(LC_ALL, "en-US.Unicode");

    printf("\e[92mdtsod parser in c language!\e[97m\n");

    test_all();

    Unitype a={Double,.Double=9};

    STNode* node=STNode_create();

    ST_push(node,"type", a);

    ST_push(node,"time", a);

    ST_push(node,"author_id", a);

    ST_push(node,"channel_id", a);

    ST_push(node,"message_id", a);

    ST_push(node,"text", a);

    ST_push(node,"files", a);

    a=ST_pull(node,"");

    //STNode_free(node);

    printf("%u\n", dhash("000"));

    printf("%u\n", dhash("0000"));

    printf("%u\n", dhash("1111"));



    

    return 0;

}

