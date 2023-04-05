#if NETSTANDARD2_1 || NET6_0 || NET7_0 || NET8_0
    #define USE_SPAN
#endif

using System.Runtime.CompilerServices;

namespace DTLib.Filesystem;

/// <summary>
/// represents filesystem path with proper dir separators and without trailing separator
/// </summary>
public readonly struct IOPath
{
    public readonly string Str;
    
    public IOPath(char[] path, bool separatorsFixed=false)
    {
        Str = separatorsFixed ? new string(path) : FixSeparators(path);
    }
    
    public IOPath(string path, bool separatorsFixed=false)
    {
        if (path.IsNullOrEmpty())
            throw new Exception("path is null or empty");
        Str = separatorsFixed ? path : FixSeparators(path.ToCharArray());
    }

    static string FixSeparators(char[] path)
    {
        int length = path.Length;
        if (path[length-1] == Path.Sep || path[length-1] == Path.NotSep)
            length--; // removing trailing sep
        char[] fixed_path = new char[length];
        for (int i = 0; i < length; i++)
        {
            if (path[i] == Path.NotSep)
                fixed_path[i] = Path.Sep;
            else fixed_path[i] = path[i];
        }
        return new string(fixed_path);
    }

    public static IOPath[] ArrayCast(string[] a, bool correct_separators=false)
    {
        IOPath[] b = new IOPath[a.Length];
        for (int i = 0; i < a.Length; i++)
            b[i] = new IOPath(a[i], correct_separators);
        return b;
    }
    public static IOPath[] ListCast(IList<string> a, bool correct_separators=false)
    {
        IOPath[] b = new IOPath[a.Count];
        for (int i = 0; i < a.Count; i++)
            b[i] = new IOPath(a[i], correct_separators);
        return b;
    }
    

    // public static IOPath operator +(IOPath a, IOPath b) => Path.Concat(a, b);
    public static bool operator ==(IOPath a, IOPath b) => a.Str==b.Str;
    public static bool operator !=(IOPath a, IOPath b) => a.Str!=b.Str;
    public static implicit operator IOPath(string s) => new(s);
    public static explicit operator string(IOPath p) => p.Str;
    public override string ToString() => Str;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        return Str == obj.ToString();
    }
    public override int GetHashCode() => Str.GetHashCode();

    public char this[int i] => Str[i];
    public int Length => Str.Length;
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char[] ToCharArray() => Str.ToCharArray();

#if  USE_SPAN
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<char> AsSpan() => Str.AsSpan();
#endif
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(char c) => Str.Contains(c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(string s) => Str.Contains(s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(IOPath p) => Str.Contains(p.Str);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool StartsWith(char c) => Str.StartsWith(c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool StartsWith(string s) => Str.StartsWith(s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool StartsWith(IOPath p) => Str.StartsWith(p.Str);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EndsWith(char c) => Str.EndsWith(c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EndsWith(string s) => Str.EndsWith(s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EndsWith(IOPath p) => Str.EndsWith(p.Str);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IOPath Remove(int startIndex) => new(Str.Remove(startIndex), true);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IOPath Remove(int startIndex, int count) => new(Str.Remove(startIndex, count), true);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IOPath Substring(int startIndex) => new(Str.Substring(startIndex), true);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IOPath Substring(int startIndex, int count) => new(Str.Substring(startIndex, count), true);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IOPath Replace(char oldChar, char newChar) => new(Str.Replace(oldChar, newChar), true);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IOPath Replace(string oldStr, string newStr) => new(Str.Replace(oldStr, newStr), true);

    ///<inheritdoc cref="string.IndexOf(char)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(char c) => Str.IndexOf(c);
    ///<inheritdoc cref="string.IndexOf(char,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(char c, int startIndex) => Str.IndexOf(c, startIndex);
    ///<inheritdoc cref="string.IndexOf(char,int,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(char c, int startIndex, int count) => Str.IndexOf(c, startIndex, count);
    ///<inheritdoc cref="string.IndexOf(string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(string s) => Str.IndexOf(s, StringComparison.Ordinal);
    ///<inheritdoc cref="string.IndexOf(string,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(string s, int startIndex) => Str.IndexOf(s, startIndex, StringComparison.Ordinal);
    ///<inheritdoc cref="string.IndexOf(string,int,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(string s, int startIndex, int count) => Str.IndexOf(s, startIndex, count, StringComparison.Ordinal);
    ///<inheritdoc cref="string.IndexOf(string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(IOPath p) => Str.IndexOf(p.Str, StringComparison.Ordinal);
    ///<inheritdoc cref="string.IndexOf(string,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(IOPath p, int startIndex) => Str.IndexOf(p.Str, startIndex, StringComparison.Ordinal);
    ///<inheritdoc cref="string.IndexOf(string,int,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(IOPath p, int startIndex, int count) => Str.IndexOf(p.Str, startIndex, count, StringComparison.Ordinal);
    
    ///<inheritdoc cref="string.LastIndexOf(char)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(char c) => Str.LastIndexOf(c);
    ///<inheritdoc cref="string.LastIndexOf(char,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(char c, int startLastIndex) => Str.LastIndexOf(c, startLastIndex);
    ///<inheritdoc cref="string.LastIndexOf(char,int,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(char c, int startLastIndex, int count) => Str.LastIndexOf(c, startLastIndex, count);
    ///<inheritdoc cref="string.LastIndexOf(string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(string s) => Str.LastIndexOf(s, StringComparison.Ordinal);
    ///<inheritdoc cref="string.LastIndexOf(string,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(string s, int startLastIndex) => Str.LastIndexOf(s, startLastIndex, StringComparison.Ordinal);
    ///<inheritdoc cref="string.LastIndexOf(string,int,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(string s, int startLastIndex, int count) => Str.LastIndexOf(s, startLastIndex, count, StringComparison.Ordinal);
    ///<inheritdoc cref="string.LastIndexOf(string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(IOPath p) => Str.LastIndexOf(p.Str, StringComparison.Ordinal);
    ///<inheritdoc cref="string.LastIndexOf(string,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(IOPath p, int startLastIndex) => Str.LastIndexOf(p.Str, startLastIndex, StringComparison.Ordinal);
    ///<inheritdoc cref="string.LastIndexOf(string,int,int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(IOPath p, int startLastIndex, int count) => Str.LastIndexOf(p.Str, startLastIndex, count, StringComparison.Ordinal);
}