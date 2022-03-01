#include "tests.h"
#include "../src/DtsodParser/DtsodV24.h"

void test_dtsod(){
    optime(__func__,1,({
        printf("\e[96m-------------[test_dtsod]-------------\n");
        char text[]=
            "message:\n"
            "{\n"
            "    type: \"sent\";\n"
            "    time: \"15.12.2021 20:51:24 +03:00\";\n"
            "    author_id: 293798876950036480ul;\n"
            "    channel_id: 913088838761603212ul;\n"
            "    message_id: 920734809096077353ul;\n"
            "    text: \"_$\\\"\\\\'''\n\ta ыыы000;2;=:%d;```\";\n"
            "};\n";
        Hashtable* dtsod=DtsodV24_deserialize(text);
    }));
}