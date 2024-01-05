global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using DTLib.Extensions;
global using DTLib.Filesystem;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace DTLib.Extensions;

public static class BaseConverter
{
    // сокращение конвертации
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBool<T>(this T input) => Convert.ToBoolean(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToChar<T>(this T input) => Convert.ToChar(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByte<T>(this T input) => Convert.ToByte(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte ToSByte<T>(this T input) => Convert.ToSByte(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ToShort<T>(this T input) => Convert.ToInt16(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ToUShort<T>(this T input) => Convert.ToUInt16(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt<T>(this T input) => Convert.ToInt32(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt<T>(this T input) => Convert.ToUInt32(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToLong<T>(this T input) => Convert.ToInt64(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToULong<T>(this T input) => Convert.ToUInt64(input);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToFloat(this string input) => float.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ToDouble<T>(this T input) => Convert.ToDouble(input, CultureInfo.InvariantCulture);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ToDecimal<T>(this T input) => Convert.ToDecimal(input, CultureInfo.InvariantCulture);
}
