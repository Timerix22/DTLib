global using DTLib;
global using DTLib.Extensions;
global using System;
using Ben.Demystifier;

namespace DTLib.Exceptions;

public static class MyExceptionHelper
{
    public static string ExToString(Exception ex)
    {
        return ex.ToStringDemystified();
    }
}
