namespace DTLib.Extensions;

public static class Collections
{
    public static void ForEach<T>(this IEnumerable<T> en, Action<T> act)
    {
        foreach (T elem in en)
            act(elem);
    }
}
