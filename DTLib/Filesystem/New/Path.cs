using System.Runtime.CompilerServices;

namespace DTLib.Filesystem.New;

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
}