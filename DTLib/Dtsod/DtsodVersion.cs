﻿namespace DTLib.Dtsod;

public enum DtsodVersion : byte
{
    V21 = 21,
    V22 = 22,
    V23 = 23
#if DEBUG
    ,V30 = 30
#endif
}
