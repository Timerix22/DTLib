#include "tests.h"
#include "../src/base/mystr.h"

void test_string(){
    optime(__func__,1,({
        printf("\e[96m-------------[test_string]-------------\n");
        char c[]="0123456789abcdef";
        string s=string_cpFromCharPtr(c);
        printf("\e[92m\"%s\" -> string_cpFromCharPtr()\n",c);
        if(s.length!=16) throw("string created with incorrect length");
        char* p=string_cpToCharPtr(s);
        printf("\e[92mstring_cpToCharPtr() -> \"%s\"\n",p);
        free(p);
        free(s.ptr);
    }));
}