namespace DTLib.Extensions;

public static class Collections
{

    public static void ForEach<T>(this IEnumerable<T> en, Action<T> act)
    {
        foreach (T elem in en)
            act(elem);
    }

    // массив в лист
    public static List<T> ToList<T>(this T[] input)
    {
        var list = new List<T>();
        list.AddRange(input);
        return list;
    }

    // удаление нескольких элементов массива
    public static T[] RemoveRange<T>(this T[] input, int startIndex, int count)
    {
        var list = input.ToList();
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
}
