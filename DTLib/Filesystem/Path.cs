namespace DTLib.Filesystem;

static public class Path
{

    public static string CorrectSeparator(string path)
    {
        if (System.IO.Path.PathSeparator == '\\')
        {
            if (path.Contains('/'))
                path = path.Replace('/', '\\');
        }
        else if (System.IO.Path.PathSeparator == '/')
        {
            if (path.Contains('\\'))
                path = path.Replace('\\', '/');
        }
        return path;
    }
}