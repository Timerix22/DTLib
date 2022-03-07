#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <stdbool.h>
#include <locale.h>
#include <time.h>

#define CHOOSE(B, Y, N) __builtin_choose_expr(B, Y, N)

#define IFTYPE(X, T) __builtin_types_compatible_p(typeof(X), T)

#define dbg(N) printf("\e[95m%d\n",N)
