#include "base/base.h"
#include "tests/tests.h"
#include "Autoarr/Autoarr.h"
#include "SearchTree/SearchTree.h"
#include "Hashtable/hash.h"

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    /*test_all();
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
    STNode_free(node);
    printf("%u\n", ihash("0"));
    printf("%lu\n", lhash("0"));
    printf("%u\n", ihash("1kakdkale210r"));
    printf("%lu\n", lhash("1kakdkale210r"));*/
    int** ptr2=NULL;
    int a=4;
    int* ptr=&a;
    ptr2=&ptr;
    printf("%p %p",ptr2, *ptr2);
    return 0;
}
