#include "tests.h"
#include "../DtsodParser/DtsodV24.h"

void test_dtsod(){
    printf("\e[96m-------------[test_dtsod]-------------\n");
    optime(__func__,1,({
        const char text[]=
            "$message:\n"
            "{\n"
            "    type: \"sent\";\n"
            "    time: \"15.12.2021 20:51:24 +03:00\";\n"
            "    author_id: 293798876950036480ul;\n"
            "    channel_id: 913088838761603212ul;\n"
            "    message_id: 920734809096077353ul;\n"
            "    text: \"_$\\\"\\\\'''\n\ta ыыы000;2;=:%d;```\";\n"
            "};\n";
        KeyValuePair id;
        Hashtable* dtsod;
        optime("deserialize",1,(dtsod=DtsodV24_deserialize(text)));
        printf("\e[92mhashtable_get(\"message\")->\n\e[94m");
        KeyValuePair msg=Hashtable_get_pair(dtsod,"message");
        printkvp(msg);
        printf("\n");
        Autoarr(Unitype)* ar=msg.value.VoidPtr;
        printf("\e[92mhashtable_get(\"message_id\")->\n\e[94m");
        id=Hashtable_get_pair(Autoarr_get(ar,0).VoidPtr,"message_id");
        printkvp(id);
        printf("\n");
        Hashtable_free(dtsod);
    }));
}