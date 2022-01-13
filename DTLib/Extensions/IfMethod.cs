namespace DTLib.Extensions;

public static class IfMethod
{
    public static T If<T>(this T input, bool condition, Func<T, T> if_true, Func<T, T> if_false) =>
        condition ? if_true(input) : if_false(input);

    public static void If<T>(this T input, bool condition, Action<T> if_true, Action<T> if_false)
    {
        if (condition) if_true(input);
        else if_false(input);
    }

    public static T If<T>(this T input, bool condition, Func<T, T> if_true) =>
        condition ? if_true(input) : input;

    public static void If<T>(this T input, bool condition, Action<T> if_true)
    {
        if (condition) if_true(input);
    }
}
