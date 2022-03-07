#include "../base/base.h"
#include "tests.h"

void test_all(){
    test_searchtree();
    test_autoarr();
    test_hashtable();
    test_string();
    test_dtsod();
    printf("\e[96m---------------------------------------\e[0m\n");
}

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    optime("test_all",1,{test_all();});
    printf("\e[0m\n");
    return 0;
}
