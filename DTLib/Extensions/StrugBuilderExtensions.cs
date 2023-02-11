namespace DTLib.Extensions;

public static class StrugBuilderExtensions
{
    public static StringBuilder AppendArray<TVal>(this StringBuilder b, TVal[] array)
    {
        for (int i = 0; i < array.Length; i++)
            b.Append(array[i]);
        return b;
    }
    public static StringBuilder AppendArray<TVal,TSep>(this StringBuilder b, TVal[] array, char separator)
    {
        if (array.Length == 0) return b;
        b.Append(array[0]);
        for (int i = 1; i < array.Length; i++)
            b.Append(separator).Append(array[i]);
        return b;
    }
    public static StringBuilder AppendArray<TVal,TSep>(this StringBuilder b, TVal[] array, string separator)
    {
        if (array.Length == 0) return b;
        b.Append(array[0]);
        for (int i = 1; i < array.Length; i++)
            b.Append(separator).Append(array[i]);
        return b;
    }

    public static StringBuilder AppendColletion<TVal>(this StringBuilder b, IEnumerable<TVal> enumerable)
    {
        foreach (var el in enumerable)
            b.Append(el);
        return b;
    }
    public static StringBuilder AppendColletion<TVal, TSep>(this StringBuilder b, IEnumerable<TVal> enumerable, char separator)
    {
        using IEnumerator<TVal> enumerator = enumerable.GetEnumerator();
        if (!enumerator.MoveNext()) return b;
        b.Append(enumerator.Current);
        while (enumerator.MoveNext())
            b.Append(separator).Append(enumerator.Current);
        return b;
    }
    public static StringBuilder AppendColletion<TVal, TSep>(this StringBuilder b, IEnumerable<TVal> enumerable, string separator)
    {
        using IEnumerator<TVal> enumerator = enumerable.GetEnumerator();
        if (!enumerator.MoveNext()) return b;
        b.Append(enumerator.Current);
        while (enumerator.MoveNext())
            b.Append(separator).Append(enumerator.Current);
        return b;
    }
}