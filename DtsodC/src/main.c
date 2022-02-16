#include "base/base.h"
#include "tests/tests.h"
#include "Autoarr/Autoarr2.h"

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    optime({test_all();});
    return 0;
}
