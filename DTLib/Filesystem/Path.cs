namespace DTLib.Filesystem;

public static class Path
{
    public static readonly char Sep = Environment.OSVersion.Platform == PlatformID.Win32NT ? '\\' : '/';

    public static string CorrectSeparator(this string path)
    {
        if (Sep == '\\')
        {
            if (path.Contains('/'))
                path = path.Replace('/', '\\');
        }
        else
        {
            if (path.Contains('\\'))
                path = path.Replace('\\', '/');
        }
        return path;
    }

    // replaces wrong characters to use string as path
    public static string NormalizeAsPath(this string str) =>
        str.Replace(':', '-').Replace(' ', '_');
}