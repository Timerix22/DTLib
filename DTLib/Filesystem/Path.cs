namespace DTLib.Filesystem;

static public class Path
{
    static public readonly char Sep = OperatingSystem.IsWindows() ? '\\' : '/';

    public static string CorrectSeparator(string path)
    {
        if (Sep == '\\')
        {
            if (path.Contains('/'))
                path = path.Replace('/', '\\');
        }
        else if (Sep == '/')
        {
            if (path.Contains('\\'))
                path = path.Replace('\\', '/');
        }
        return path;
    }
}