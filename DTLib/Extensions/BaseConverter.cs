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
#if NETSTANDARD2_1 || NET6_0 || NET7_0 || NET8_0
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToFloat(this ReadOnlySpan<char> input) => float.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
#endif
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ToDouble<T>(this T input) => Convert.ToDouble(input, CultureInfo.InvariantCulture);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ToDecimal<T>(this T input) => Convert.ToDecimal(input, CultureInfo.InvariantCulture);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this byte[] bytes)
    {
        int output = 0;
        for (ushort i = 0; i < bytes.Length; i++)
            output = output * 256 + bytes[i];
        return output;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] IntToBytes(this int num)
    {
        List<byte> output = new();
        while (num != 0)
        {
            output.Add((byte)(num % 256));
            num = (int)(num / 256.0);
        }
        output.Reverse();
        return output.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TruncateToInt(this double number) => Math.Truncate(number).ToInt();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TruncateToInt(this decimal number) => Math.Truncate(number).ToInt();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long TruncateToLong(this double number) => Math.Truncate(number).ToLong();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long TruncateToLong(this decimal number) => Math.Truncate(number).ToLong();
}
