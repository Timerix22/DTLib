#ifndef __ERR_AUTOARR_DEFINED
#define __ERR_AUTOARR_DEFINED
typedef enum err_t {
    SUCCESS, ERR_MAXLENGTH, ERR_WRONGTYPE, ERR_WRONGINDEX, ERR_NOTFULL
} err_t;

void throwstr(const char* errmesg);
void throwerr(err_t err);
#endif