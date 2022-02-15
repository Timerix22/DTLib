#include "base/base.h"
#include "tests/tests.h"

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    //test_all();
    test_new();
    return 0;
}
