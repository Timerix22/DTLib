using System.Runtime.CompilerServices;

namespace DTLib.Filesystem;

public static class Path
{
    public static readonly char Sep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '\\' : '/';
    public static readonly char NotSep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '/' : '\\' ;
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfEscapes(this IOPath path)
    {
        if (path.Str.Contains(".."))
            throw new Exception($"path <{path}> uses <..>, that's not allowed");
    }
    
    /// Replaces characters restricted in filesystem path
    public static IOPath ReplaceRestrictedChars(string str)
    {
        char[] r = str.ToCharArray();
        StringBuilder b = new(r.Length);
        for (int i = 0; i < str.Length; i++)
        {
            char c = r[i];
            switch (c)
            {
                case '\n': case '\r':
                case ':': case ';':
                    break;
                case '/': case '\\':
                    b.Append('-');
                    break;
                case '<': case '>': 
                case '?': case '|':
                    b.Append('_');
                    break; 
                case '"':
                    b.Append('\'');
                    break;
                case '*':
                    b.Append('X');
                    break;
                default:
                    b.Append(c);
                    break;
            }
        }
        return new IOPath(b.ToString(), true);
    }

#if  !USE_SPAN
    private static void  CopyTo(this string s, char[] b, int startIndex)
    {
        for (int i = 0; i < s.Length; i++)
            b[startIndex+i] = s[i];
    }
#endif
    
    public static IOPath Concat(params IOPath[] parts)
    {
        var needSeparator = new bool[parts.Length-1];
        int lengthSum = 0;
        for (int i = 0; i < parts.Length-1; i++)
        {
            lengthSum += parts[i].Length;
            if (!parts[i].Str.EndsWith(Sep) && !parts[i + 1].Str.StartsWith(Sep))
            {
                needSeparator[i] = true;
                lengthSum++;
            }
            else needSeparator[i] = false;
        }
        lengthSum += parts[parts.Length-1].Length;
        var buffer = new char[lengthSum];
        parts[0].Str.CopyTo(buffer, 0);
        int copiedChars = parts[0].Length;
        for (int i = 1; i < parts.Length; i++)
        {
            if (needSeparator[i-1])
                buffer[copiedChars++] = Sep;
            parts[i].Str.CopyTo(buffer, copiedChars);
            copiedChars += parts[i].Length;
        }
    
        return new IOPath(new string(buffer), true);
    }

    /// returns just dir name or file name with extension
    public static IOPath LastName(this IOPath path)
    {
        int i = path.LastIndexOf(Sep);
        if (i == path.Length - 1) // ends with separator
            i = path.LastIndexOf(Sep, i-1);
        if (i == -1) return path;
        return path.Substring(i+1);
    }
    
    public static IOPath Extension(this IOPath path)
    {
        int i = path.LastIndexOf('.');
        if (i == -1) return LastName(path);
        return path.Substring(i + 1);
    }

    public static IOPath ParentDir(this IOPath path)
    {
        int i = path.LastIndexOf(Sep);
        if (i == path.Length - 1) // ends with separator
            i = path.LastIndexOf(Sep, i-1);
        if (i == -1) // no parent dir
            return $".{Sep}";
        return path.Remove(i+1);
    }

    public static IOPath ReplaceBase(this IOPath path, IOPath baseDir, IOPath otherDir)
    {
        if (!path.StartsWith(baseDir))
            throw new Exception($"path <{path}> doesnt starts with <{baseDir}");
        return Concat(otherDir, path.Substring(baseDir.Length));
    }

    public static IOPath RemoveBase(this IOPath path, IOPath baseDir)
    {
        if (!path.StartsWith(baseDir))
            throw new Exception($"path <{path}> doesnt starts with <{baseDir}");
        return path.Substring(baseDir.Length+1);
    }
}