#pragma once

typedef enum err_t {
    SUCCESS, //not an error 
    ERR_MAXLENGTH, ERR_WRONGTYPE, ERR_WRONGINDEX, ERR_NOTIMPLEMENTED, ERR_NULLPTR
} err_t;

const char* errname(err_t err);

void _throwint(int err, const char* srcfile, int line, const char* funcname);
void _throwstr(const char* errmesg, const char* srcfile, int line, const char* funcname);
#pragma GCC diagnostic ignored "-Wint-conversion"
#define throw(E) \
    CHOOSE(IFTYPE(E,int), _throwint(E,__FILE__,__LINE__,__func__), \
    CHOOSE(IFTYPE(E,char[]), _throwstr(E,__FILE__,__LINE__,__func__), \
    printf("\e[31m[%s:%d/%s] UNKNOWN ERROR\n",__FILE__,__LINE__,__func__)))
