#include "../src/base/base.h"
#include "tests.h"


void printuni(Unitype v){
    switch (v.type) {
        case Null: printf("{Null}");break;
        case Double: printf("{%s:%lf}",typename(v.type),v.Double);break;
        case Char: printf("{%s:%c}",typename(v.type),v.Char);break;
        case Bool:
        case UInt64: printf("{%s:%lu}",typename(v.type),v.UInt64);break;
        case Int64: printf("{%s:%ld}",typename(v.type),v.Int64);break;
        default: printf("{%s:%p}",typename(v.type),v.VoidPtr);break;
    }
}

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
