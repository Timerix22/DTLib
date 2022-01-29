#include "std.h"
#include "errors.h"

const char* errname(err_t err){
    switch(err){
        case SUCCESS: return "SUCCESS";
        case ERR_MAXLENGTH: return "ERR_MAXLENGTH";
        case ERR_WRONGTYPE: return "ERR_WRONGTYPE";
        case ERR_WRONGINDEX: return "ERR_WRONGINDEX";
        case ERR_NOTIMPLEMENTED: return "ERR_NOTIMPLEMENTED";
        default: return "UNKNOWN_ERROR";
    }
}

void _throwint(int err, const char* srcfile, int line, const char* funcname){
    printf("\e[31m[%s:%d/%s] throwed error: %s\e[0m\n",srcfile,line,funcname,errname(err));
    exit(err); 
}
void _throwstr(const char* errmesg, const char* srcfile, int line, const char* funcname){
    printf("\e[31m[%s:%d/%s] throwed error: %s\e[0m\n",srcfile,line,funcname,errmesg);
    exit(255); 
}