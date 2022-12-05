using System.IO;

namespace DTLib.Experimental;

public class FileInstance
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
    
    public FileInstance(string path, FileOpenMode mode)
    {
        if (path.IsNullOrEmpty())
            throw new NullReferenceException("path is null");
        if(!System.IO.File.Exists(path))
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
}