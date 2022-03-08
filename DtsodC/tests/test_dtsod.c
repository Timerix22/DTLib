#include "tests.h"
#include "../DtsodParser/DtsodV24.h"

const char text[]=
"message: {\n"
"    bool: false;"
"    char: 'v';"
"    int: -2515;"
"    uint:#comment!\n 0u;"
"    double: 965.557f;#another comment!\n"
"    text: \"_$\\\"\\\\'''a ыыы000;2;=:%d;```\";\n"
"}; ";

void test_dtsod(){
    printf("\e[96m-------------[test_dtsod]-------------\n");
    optime(__func__,200,({
        Hashtable* dtsod;
        optime("deserialize",1,(dtsod=DtsodV24_deserialize(text)));
        Hashtable_foreach(dtsod, p,({
            printkvp(p);
            if(p.value.type==HashtablePtr){
                printf(":\n{\n");
                Hashtable* sub=p.value.VoidPtr;
                Hashtable_foreach(sub, _p,({
                    printf("    ");
                    printkvp(_p);
                    printf("\n");
                }));
                printf("}");
            }
            printf("\n");

            char* s=DtsodV24_serialize(dtsod);
            printf("\e[93m\n%s",s);
            dtsod=DtsodV24_deserialize(s);
            Hashtable_free(dtsod);
        }));

    }));
}