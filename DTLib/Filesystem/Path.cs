using System.Runtime.CompilerServices;

namespace DTLib.Filesystem;

public static class Path
{
    
    public static readonly char Sep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '\\' : '/';
    private static readonly char NotSep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '/' : '\\' ;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Concat(string path, string addition) => $"{path}{Sep}{addition}";
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Concat(params string[] parts) => StringConverter.MergeToString(Sep, parts);

    public static string FixSeparators(string path)
    {
        var chars = path.ToCharArray();
        int length = path.Length;
        for(int i=0; i<length; i++)
            if (chars[i] == NotSep)
                chars[i] = Sep;
        return chars.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Resolve(params string[] parts) => FixSeparators(Concat(parts));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Resolve(string path, string addition) => FixSeparators(Concat(path, addition));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfEscapes(string path)
    {
        if (path.Contains(".."))
            throw new Exception($"path <{path}> uses <..>, that's not allowed");
    }

    /// Replaces restricted characters in string
    public static string CorrectString(string str)
    {
#if  NETSTANDARD2_1 || NET6_0 || NET7_0 || NET8_0
        var a = str.AsSpan();
#else
        var a = str.ToArray();
#endif
        char[] r = new char[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            switch (a[i])
            {
                case '/': case '\\':
                case ':': case ';':
                    r[i] = '-';
                    break;
                case '\n': case '\r': case ' ':
                case '#': case '%': case '&':
                case '{': case '}': case '<':
                case '>': case '*': case '?':
                case '$': case '!': case '\'':
                case '"': case '@': case '+':
                case '`': case '|': case '=':
                    r[i] = '_';
                    break;
                default:
                    r[i] = a[i];
                    break;
            }
        }

        return new string(r);
    }
}