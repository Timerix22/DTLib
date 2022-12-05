using System.IO;

namespace DTLib.Filesystem.New;

public class File
{
    public enum FileOpenMode
    {
        // open a file for reading 
        Read=1, 
        // (re)create a file for writing
        Write=2, 
        // opens file for writing additional data to the end / creates new file 
        Append=4,
        // (re)creates file for reading/writing
        ReadWrite=Read|Write, 
        // opens file for readng/writing additional data to the end / creates new file 
        ReadAppend=Read|Append
    }

    public readonly FileOpenMode Mode;
    public readonly FileStream Stream;
    public string Name;
    
    public File(string path, FileOpenMode mode)
    {
        if(!Exists(path))
        {
            if (mode == FileOpenMode.Read)
                throw new Exception($"file <{path}> is not found");
        }
        Mode = mode;
        Stream = mode switch
        {
            FileOpenMode.Read => System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
            FileOpenMode.Write => System.IO.File.Open(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite),
            FileOpenMode.Append => System.IO.File.Open(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite),
            FileOpenMode.ReadWrite => System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite),
            FileOpenMode.ReadAppend => System.IO.File.Open(path, FileMode.Append, FileAccess.ReadWrite, FileShare.ReadWrite),
            _ => throw new Exception($"unknown file mode: {mode}")
        };
    }

    public static bool Exists(string path) => System.IO.File.Exists(path);
    
    public static void Create(string path)
    {
        if (Exists(path)) 
            throw new Exception($"file <{path} already exists");
        int sepIndex = path.LastIndexOf(Path.Sep);
        if (sepIndex>-1)
            Directory.Create(path.Remove(sepIndex));
        System.IO.File.Create(path).Close();
    }
    
    public static void Delete(string path) => System.IO.File.Delete(path);

    public static void Copy(string srcPath, string newPath, bool replace = false)
    {
        System.IO.File.Copy(srcPath, newPath, replace);
    }

    // public void Delete(string path)
}