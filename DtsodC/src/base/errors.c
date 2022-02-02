#include "std.h"
#include "errors.h"

const char* errname(err_t err){
    switch(err){
        case SUCCESS: return "SUCCESS";
        case ERR_MAXLENGTH: return "ERR_MAXLENGTH";
        case ERR_WRONGTYPE: return "ERR_WRONGTYPE";
        case ERR_WRONGINDEX: return "ERR_WRONGINDEX";
        case ERR_NOTIMPLEMENTED: return "ERR_NOTIMPLEMENTED";
        case ERR_NULLPTR: return "ERR_NULLPTR";
        default: return "UNKNOWN_ERROR";
    }
}

void _throwint(int err, const char* srcfile, int line, const char* funcname){
    if(err){ // SUCCESS=0 is not an error
        printf("\e[91m[%s:%d/%s] throwed error: %s\e[0m\n",srcfile,line,funcname,errname(err));
        exit(err); 
    }
    else printf("\e[93m[%s:%d/%s] throwed SUCCESS as an error",srcfile,line,funcname);
}
void _throwstr(const char* errmesg, const char* srcfile, int line, const char* funcname){
    printf("\e[91m[%s:%d/%s] throwed error: %s\e[0m\n",srcfile,line,funcname,errmesg);
    exit(255); 
}