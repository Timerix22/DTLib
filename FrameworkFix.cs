using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

// включает init и record из c# 9.0
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}

namespace DTLib
{
    //
    // содержит методы расширения для различных операций и преобразований
    //
    public static class FrameworkFix
    {

        // эти методы работают как надо, в отличии от стандартных, которые иногда дуркуют
        public static bool StartsWith(this byte[] source, byte[] startsWith)
        {
            for (int i = 0; i < startsWith.Length; i++)
            {
                if (source[i] != startsWith[i])
                    return false;
            }
            return true;
        }

        public static bool EndsWith(this byte[] source, byte[] endsWith)
        {
            for (int i = 0; i < endsWith.Length; i++)
            {
                if (source[source.Length - endsWith.Length + i] != endsWith[i])
                    return false;
            }
            return true;
        }

        // Math.Truncate принимает как decimal, так и doublе,
        // из-за чего вызов метода так: Math.Truncate(10/3) выдаст ошибку "неоднозначный вызов"
        public static int Truncate<T>(this T number) => Math.Truncate(number.ToDouble()).ToInt();

        // массив в лист
        public static List<T> ToList<T>(this T[] input)
        {
            List<T> list = new List<T>();
            list.AddRange(input);
            return list;
        }

        // удаление нескольких элементов массива
        public static T[] RemoveRange<T>(this T[] input, int startIndex, int count)
        {
            List<T> list = input.ToList();
            list.RemoveRange(startIndex, count);
            return list.ToArray();
        }
        public static T[] RemoveRange<T>(this T[] input, int startIndex) => input.RemoveRange(startIndex, input.Length - startIndex);

        // метод как у листов
        public static bool Contains<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(value))
                    return true;
            return false;
        }

        // сокращение конвертации
        public static int ToInt<T>(this T input) => Convert.ToInt32(input);
        public static uint ToUInt<T>(this T input) => Convert.ToUInt32(input);
        public static long ToLong<T>(this T input) => Convert.ToInt64(input);
        public static ulong ToULong<T>(this T input) => Convert.ToUInt64(input);
        public static short ToShort<T>(this T input) => Convert.ToInt16(input);
        public static ushort ToUShort<T>(this T input) => Convert.ToUInt16(input);
        public static double ToDouble<T>(this T input) => Convert.ToDouble(input, System.Globalization.CultureInfo.InvariantCulture);
        public static byte ToByte<T>(this T input) => Convert.ToByte(input);
        public static sbyte ToSByte<T>(this T input) => Convert.ToSByte(input);
        public static bool ToBool<T>(this T input) => Convert.ToBoolean(input);

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
                num = Truncate(num / 256);
            }
            output.Reverse();
            return output.ToArray();
        }
        public static byte[] ToBytes(this string str) => UTF8.GetBytes(str);

        public static Encoding UTF8 = new UTF8Encoding(false);
        // байты в кодировке UTF8 в строку
        public static string BytesToString(this byte[] bytes) => UTF8.GetString(bytes);

        // хеш в виде массива байт в строку (хеш изначально не в кодировке UTF8, так что метод выше не работает с ним)
        public static string HashToString(this byte[] hash)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string MergeToString(params object[] parts)
        {
            StringBuilder builder = new();
            for (int i = 0; i < parts.Length; i++)
                builder.Append(parts[i].ToString());
            return builder.ToString();
        }
        public static string MergeToString<T>(this IEnumerable<T> collection, string separator)
        {
            StringBuilder builder = new();
            foreach (T elem in collection)
            {
                builder.Append(elem.ToString());
                builder.Append(separator);
            }
            if (builder.Length == 0)
                return "";
            builder.Remove(builder.Length - separator.Length, separator.Length);
            return builder.ToString();
        }
        public static string MergeToString<T>(this IEnumerable<T> collection)
        {
            StringBuilder builder = new();
            foreach (T elem in collection)
                builder.Append(elem.ToString());
            return builder.ToString();
        }

        public static string Multiply(this string input, int howMany)
        {
            StringBuilder b = new();
            for (int i = 0; i < howMany; i++)
                b.Append(input);
            return b.ToString();
        }

        public static void Throw(this Exception ex) => throw ex;

        public static void ForEach<T>(this IEnumerable<T> en, Action<T> act)
        {
            foreach (T elem in en)
                act(elem);
        }

        // делает что надо в отличии от String.Split(), который не убирает char c из начала
        public static List<string> SplitToList(this string s, char c)
        {
            char[] ar = s.ToCharArray();
            StringBuilder b = new();
            List<string> o = new();
            if (ar[0] != c)
                b.Append(ar[0]);
            for (int i = 1; i < ar.Length; i++)
                if (ar[i] == c)
                {
                    if (b.Length > 0)
                        o.Add(b.ToString());
                    b.Clear();
                }
                else
                    b.Append(ar[i]);
            if (b.Length > 0)
                o.Add(b.ToString());
            return o;
        }

        // разбивает на части указанной длины
        public static List<string> Split(this string s, int length)
        {
            List<string> parts = new();
            int max = (s.Length / length).Truncate();
            for (int i = 0; i < max; i++)
                parts.Add(s.Substring(i * length, length));
            if (max * length != s.Length) parts.Add(s.Substring(max * length, s.Length - max * length));
            return parts;
        }

        public static T If<T>(this T input, bool condition, Func<T, T> if_true, Func<T, T> if_false) =>
            condition ? if_true(input) : if_false(input);
    }
}