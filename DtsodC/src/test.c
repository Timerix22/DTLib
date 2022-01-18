#ifndef STDLIB
    #include "../cosmopolitan/cosmopolitan.h"
#else
    #include <stdio.h>
    #include <stdlib.h>
    #include <stdbool.h>
    #include <locale.h>
    #include <uchar.h>
    #include <wchar.h>
    #include <sys/stat.h>
    #include <time.h>
#endif
#include "strict_types.h"
#include "autosize_array/Autoarr.h"
void nsleep(long miliseconds) {
    struct timespec ts = {10, miliseconds * 1000000L};
    nanosleep(&ts, NULL);
}
int8 main(){
    printf("\e[32mdtsod parser in c language!\n");
    dsleep(0.5);
    //Autoarr* ar=Autoarr_create(100,10,Int8);
    //Autoarr_add_int8(&ar,-1);
    //printf("ar[0]==%d",Autoarr_get_int8(&ar,0));
    //free(ar);
    //setlocale(LC_ALL, "en-US.Unicode");
    /*struct stat st;
    FILE* f1 = fopen("1.gui","r");
    FILE* f2 = fopen("2.gui","r");
    fstat(fileno(f1), &st);
    size_t size = st.st_size;
    char page1[size];
    fread(page1, 8, size, f1);
    char page2[size];
    fread(page2, 8, size, f2);
    printf("\e[31m\n");
    for (int64 i = 0; i<100000; i++){
        nsleep(10000L);
        if(i%2==0) printf("%s", page1);
        else printf("%s", page2);
    }*/
    printf("\e[32m\n");
    //getc(stdin);
    return 0;
}