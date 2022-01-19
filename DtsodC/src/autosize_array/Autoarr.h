#include "../!headers.h"
#include "../strict_types.h"
#include "../errors.h"

typedef struct Autoarr{
    void** values;
    strict_type type;
    uint16 max_block_length;
    uint16 curr_block_length;
    uint16 block_count;
    uint16 max_block_count;
} Autoarr;
typedef Autoarr Autoarr;


Autoarr* Autoarr_create(uint16 max_length, uint16 max_block_length, strict_type type);

uint32 Autoarr_length(Autoarr *a);
uint32 Autoarr_max_length(Autoarr *a);

void Autoarr_add(Autoarr *autoarr, void *element);

void Autoarr_set(Autoarr *autoarr, uint32 index, void *element);

void Autoarr_remove(Autoarr *autoarr, uint32 index);

void Autoarr_removeRange(Autoarr *autoarr, uint32 startIndex, uint32 length);

int8 Autoarr_get_int8(Autoarr *autoarr, uint32 index);
uint8 Autoarr_get_uint8(Autoarr *autoarr, uint32 index);
int16 Autoarr_get_int16(Autoarr *autoarr, uint32 index);
uint16 Autoarr_get_uint16(Autoarr *autoarr, uint32 index);
int32 Autoarr_get_int32(Autoarr *autoarr, uint32 index);
uint32 Autoarr_get_uint32(Autoarr *autoarr, uint32 index);
int64 Autoarr_get_int64(Autoarr *autoarr, uint32 index);
uint64 Autoarr_get_uint64(Autoarr *autoarr, uint32 index);