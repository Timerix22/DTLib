global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using DTLib.Extensions;
global using DTLib.Filesystem;
global using static DTLib.PublicLog;

namespace DTLib.Extensions;

public static class BaseConverter
{
    // сокращение конвертации
    public static bool ToBool<T>(this T input) => Convert.ToBoolean(input);
    public static char ToChar<T>(this T input) => Convert.ToChar(input);
    public static byte ToByte<T>(this T input) => Convert.ToByte(input);
    public static sbyte ToSByte<T>(this T input) => Convert.ToSByte(input);
    public static short ToShort<T>(this T input) => Convert.ToInt16(input);
    public static ushort ToUShort<T>(this T input) => Convert.ToUInt16(input);
    public static int ToInt<T>(this T input) => Convert.ToInt32(input);
    public static uint ToUInt<T>(this T input) => Convert.ToUInt32(input);
    public static long ToLong<T>(this T input) => Convert.ToInt64(input);
    public static ulong ToULong<T>(this T input) => Convert.ToUInt64(input);
    public static float ToFloat(this string input) => float.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
    public static double ToDouble<T>(this T input) => Convert.ToDouble(input, System.Globalization.CultureInfo.InvariantCulture);
    public static decimal ToDecimal<T>(this T input) => Convert.ToDecimal(input, System.Globalization.CultureInfo.InvariantCulture);

    public static int ToInt(this byte[] bytes)
    {
        int output = 0;
        for (ushort i = 0; i < bytes.Length; i++)
            output = output * 256 + bytes[i];
        return output;
    }

    public static byte[] ToBytes(this int num)
    {
        List<byte> output = new();
        while (num != 0)
        {
            output.Add(ToByte(num % 256));
            num = (num / 256).Truncate();
        }
        output.Reverse();
        return output.ToArray();
    }

    // Math.Truncate принимает как decimal, так и doublе,
    // из-за чего вызов метода так: Math.Truncate(10/3) выдаст ошибку "неоднозначный вызов"
    public static int Truncate<T>(this T number) => Math.Truncate(number.ToDouble()).ToInt();
}
