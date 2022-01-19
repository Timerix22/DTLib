#include "!headers.h"
#include "strict_types.h"
#include "errors.h"
#include "autosize_array/Autoarr.h"
#include "nsleep.h"
#include "slimak.h"

#define clrscr() printf("\e[1;1H\e[2J")
int main(){
    printf("\e[32mdtsod parser in c language!\n");
    //Autoarr* ar=Autoarr_create(100,10,Int8);
    //Autoarr_add_int8(&ar,-1);
    //printf("ar[0]==%d",Autoarr_get_int8(&ar,0));
    //free(ar);
    setlocale(LC_ALL, "en-US.Unicode");
    uint8 n = 0;
    clrscr();
    for (uint16 i = 0; i<40; i++){
        nsleep(0,70);
        printf("\e[1;1H%.*ls", 105,slimak+n*105);
        if (n>=10) n=0;
        else n++;
    }
    for (uint16 i = 0; i<120; i++){
        nsleep(0,20);
        printf("\e[1;1H%.*ls", 105,slimak+n*105);
        if (n>=10) n=0;
        else n++;
    }
    clrscr();
    printf("\e[32m\npress any key to exit...\n");
    getc(stdin);
    return 0;
}