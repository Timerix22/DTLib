namespace DTLib.Filesystem;

public static class File
{
    public static int GetSize(string file) => new System.IO.FileInfo(file).Length.ToInt();

    public static bool Exists(string file) => System.IO.File.Exists(file);

    // если файл не существует, создаёт файл, создаёт папки из его пути
    public static void Create(string file, bool delete_old = false)
    {
        if (delete_old && File.Exists(file))
            File.Delete(file);
        if (!File.Exists(file))
        {
            if (file.Contains("\\"))
                Directory.Create(file.Remove(file.LastIndexOf('\\')));
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
        using System.IO.FileStream stream = File.OpenRead(file);
        int size = GetSize(file);
        byte[] output = new byte[size];
        stream.Read(output, 0, size);
        stream.Close();
        return output;
    }

    public static string ReadAllText(string file) => ReadAllBytes(file).BytesToString();

    public static void WriteAllBytes(string file, byte[] content)
    {
        using System.IO.FileStream stream = File.OpenWrite(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void WriteAllText(string file, string content) => WriteAllBytes(file, content.ToBytes());

    public static void AppendAllBytes(string file, byte[] content)
    {
        using System.IO.FileStream stream = File.OpenAppend(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void AppendAllText(string file, string content) => AppendAllBytes(file, content.ToBytes());

    public static System.IO.FileStream OpenRead(string file) =>
        Exists(file) ? System.IO.File.OpenRead(file) : throw new Exception($"file not found: <{file}>");
    public static System.IO.FileStream OpenWrite(string file)
    {
        File.Create(file, true);
        return System.IO.File.Open(file, System.IO.FileMode.OpenOrCreate);
    }
    public static System.IO.FileStream OpenAppend(string file)
    {
        File.Create(file);
        return System.IO.File.Open(file, System.IO.FileMode.Append);
    }

    public static void CreateSymlink(string sourceName, string symlinkName)
    {
        if (symlinkName.Contains("\\"))
            Directory.Create(symlinkName.Remove(symlinkName.LastIndexOf('\\')));
        if (!Symlink.CreateSymbolicLink(symlinkName, sourceName, Symlink.SymlinkTarget.File))
            throw new InvalidOperationException($"some error occured while creating symlink\nFile.CreateSymlink({symlinkName}, {sourceName})");
    }
}
