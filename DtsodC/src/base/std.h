#pragma once

#if COSMOPOLITAN
    #include "../../cosmopolitan/cosmopolitan.h"
#else
    #include <stdio.h>
    #include <stdlib.h>
    #include <stdint.h>
    #include <stdbool.h>
    #include <locale.h>
    #include <uchar.h>
    #include <wchar.h>
    #include <sys/stat.h>
    #include <time.h>
    #include <unistd.h>
#endif

#define CHOOSE(B, Y, N) __builtin_choose_expr(B, Y, N)
#define IFTYPE(X, T) __builtin_types_compatible_p(typeof(X), T)
