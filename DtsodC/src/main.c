#include "base/base.h"
#include "tests/tests.h"
#include "Autoarr/Autoarr2.h"
#include "Hashtable/Hashtable.h"

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    optime("test_all",1,{test_all();});
    //test_hashtable();
    printf("\e[0m\n");
    return 0;
}
