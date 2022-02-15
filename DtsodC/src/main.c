#include "base/base.h"
#include "tests/tests.h"
#include "Autoarr/Autoarr.h"
#include "Autoarr/Autoarr2.h"

int main(){
    setlocale(LC_ALL, "en-US.Unicode");
    printf("\e[92mdtsod parser in c language!\e[97m\n");
    optime({test_all();});
    optime({
        Autoarr2(uint64) _ar2=Autoarr2_create(uint64,10000,20000);
        Autoarr2(uint64)* ar2=&_ar2;
        for(uint32 i=0;i<Autoarr2_max_length(ar2);i++)
            Autoarr2_add(ar2,8);
    });
    optime({
        Autoarr _ar=Autoarr_create(10000,20000,UInt64);
        Autoarr* ar=&_ar;
        for(uint32 i=0;i<ar->max_length;i++)
            Autoarr_add_uint64(ar,8);
    });
    return 0;
}
