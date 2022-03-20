namespace DTLib.Extensions;

public static class StringConverter
{
    public static Encoding UTF8 = new UTF8Encoding(false);
    public static byte[] ToBytes(this string str) => UTF8.GetBytes(str);
    public static string BytesToString(this byte[] bytes) => UTF8.GetString(bytes);

    // хеш в виде массива байт в строку (хеш изначально не в кодировке UTF8, так что метод выше не работает с ним)
    public static string HashToString(this byte[] hash)
    {
        var builder = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            builder.Append(hash[i].ToString("x2"));
        }
        return builder.ToString();
    }


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

    public static bool StartsWith(this string s, char c) => s[0] == c;
    public static bool EndsWith(this string s, char c) => s[s.Length - 1] == c;

    public static string MergeToString(params object[] parts)
    {
        StringBuilder builder = new();
        for (int i = 0; i < parts.Length; i++)
            builder.Append(parts[i]);
        return builder.ToString();
    }
    public static string MergeToString<T>(this IEnumerable<T> collection, string separator)
    {
        StringBuilder builder = new();
        foreach (T elem in collection)
        {
            builder.Append(elem);
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
            builder.Append(elem);
        return builder.ToString();
    }

    public static string Multiply(this string input, int howMany)
    {
        StringBuilder b = new();
        for (int i = 0; i < howMany; i++)
            b.Append(input);
        return b.ToString();
    }
    public static string Multiply(this char input, int howMany)
    {
        StringBuilder b = new();
        for (int i = 0; i < howMany; i++)
            b.Append(input);
        return b.ToString();
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
            else b.Append(ar[i]);
        if (b.Length > 0) o.Add(b.ToString());
        return o;
    }

    // правильно реагирует на кавычки
    public static List<string> SplitToList(this string s, char c, char quot)
    {
        List<string> output = new();
        List<string> list = s.SplitToList(c);
        bool q_open = false;
        for (int i = 0; i < list.Count; i++)
        {
            string _s = list[i];
            if (q_open)
            {
                if (_s.EndsWith(quot))
                {
                    q_open = false;
                    _s = _s.Remove(_s.Length - 1);
                }
                output[output.Count - 1] += c + _s;
            }
            else if (_s.StartsWith(quot))
            {
                q_open = true;
                _s = _s.Remove(0, 1);
            }
            output.Add(_s);
        }
        return output;
    }

    // разбивает на части указанной длины
    public static List<string> SplitToList(this string s, int length)
    {
        List<string> parts = new();
        int max = (s.Length / length).Truncate();
        for (int i = 0; i < max; i++)
            parts.Add(s.Substring(i * length, length));
        if (max * length != s.Length) parts.Add(s.Substring(max * length, s.Length - max * length));
        return parts;
    }
}
