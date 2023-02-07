using System.Runtime.CompilerServices;

namespace DTLib.Filesystem;

public static class Path
{
    
    public static readonly char Sep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '\\' : '/';
    private static readonly char NotSep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '/' : '\\' ;
    
    /// does not correct separators, use Resolve for correction
    /// <see cref="Resolve(string[])"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Concat(string path, string addition)
    {
        if (!path.EndsWith(Sep) && !addition.StartsWith(Sep))
            path += Sep;
        return path + addition;
    }

    /// <inheritdoc cref="Concat(string,string)"/>
    public static string Concat(params string[] parts)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(parts[0]);
        for (int i = 1; i < parts.Length; i++)
        {
            char lastC = builder[builder.Length - 1];
            if(lastC!=Sep && lastC!=NotSep)
                builder.Append(Sep);
            builder.Append(parts[i]);
        }
        return builder.ToString();
    }

    public static string FixSeparators(string path)
    {
        var chars = path.ToCharArray();
        int length = path.Length;
        for(int i=0; i<length; i++)
            if (chars[i] == NotSep)
                chars[i] = Sep;
        return new string(chars);
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

    public static string FileName(string path, bool separatorsFixed)
    {
        if(!separatorsFixed)
            path = FixSeparators(path);
        int i = path.LastIndexOf(Sep);
        if (i == -1) return path;
        return path.Substring(i+1);
    }
    
    public static string Extension(string path)
    {
        int i = path.LastIndexOf('.');
        if (i == -1) return FileName(path);
        return path.Substring(i + 1);
    }

    public static string ParentDir(string path, bool separatorsFixed)
    {
        if(!separatorsFixed)
            path = FixSeparators(path);
        int i = path.LastIndexOf(Sep);
        if (i == path.Length - 1) // ends with separator
            i = path.LastIndexOf(Sep, 0, i);
        if (i == -1) // no parent dir
            return $".{Sep}";
        return path.Substring(0, i);
    }

    public static string ReplaceBase(string path, string baseDir, string otherDir)
    {
        if (!path.StartsWith(baseDir))
            throw new Exception($"path <{path}> doesnt starts with <{baseDir}");
        return Concat(otherDir, path.Substring(baseDir.Length));
    }
}