
namespace DTLib.Filesystem;

public static class File
{
    public static int GetSize(string file) => new System.IO.FileInfo(file).Length.ToInt();

    public static bool Exists(string file) => System.IO.File.Exists(file);

    // если файл не существует, создаёт файл, создаёт папки из его пути
    public static void Create(string file)
    {
        if (!Exists(file))
        {
            if (file.Contains(Путь.Разд))
                Directory.Create(file.Remove(file.LastIndexOf(Путь.Разд)));
            using System.IO.FileStream stream = System.IO.File.Create(file);
            stream.Close();
        }
    }

    public static void Copy(string srcPath, string newPath, bool replace = false)
    {
        if (!replace && Exists(newPath))
            throw new Exception($"file <{newPath}> alredy exists");
        Create(newPath);
        WriteAllBytes(newPath, ReadAllBytes(srcPath));
    }

    public static void Delete(string file) => System.IO.File.Delete(file);

    public static byte[] ReadAllBytes(string file)
    {
        using System.IO.FileStream stream = OpenRead(file);
        int size = GetSize(file);
        byte[] output = new byte[size];
        stream.Read(output, 0, size);
        stream.Close();
        return output;
    }

    public static string ReadAllText(string file) => ReadAllBytes(file).BytesToString();

    public static void WriteAllBytes(string file, byte[] content)
    {
        using System.IO.FileStream stream = OpenWrite(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void WriteAllText(string file, string content) => WriteAllBytes(file, content.ToBytes());

    public static void AppendAllBytes(string file, byte[] content)
    {
        using System.IO.FileStream stream = OpenAppend(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void AppendAllText(string file, string content) => AppendAllBytes(file, content.ToBytes());

    public static System.IO.FileStream OpenRead(string file) =>
        Exists(file) 
            ? System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite) 
            : throw new Exception($"file not found: <{file}>");
    public static System.IO.FileStream OpenWrite(string file)
    {
        if (Exists(file))
            Delete(file);
        Create(file);
        return System.IO.File.Open(file, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
    }
    public static System.IO.FileStream OpenAppend(string file)
    {
        Create(file);
        return System.IO.File.Open(file, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
    }

    public static void CreateSymlink(string sourceName, string symlinkName)
    {
        if (symlinkName.Contains(Путь.Разд))
            Directory.Create(symlinkName.Remove(symlinkName.LastIndexOf(Путь.Разд)));
        if (!Symlink.CreateSymbolicLink(symlinkName, sourceName, Symlink.SymlinkTarget.File))
            throw new InvalidOperationException($"some error occured while creating symlink\nFile.CreateSymlink({symlinkName}, {sourceName})");
    }
}
