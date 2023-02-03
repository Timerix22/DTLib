
namespace DTLib.Filesystem;

public static class File
{
    /// возвращает размер файла в байтах
    public static long GetSize(string file) => new System.IO.FileInfo(Path.FixSeparators(file)).Length;

    public static bool Exists(string file) => System.IO.File.Exists(Path.FixSeparators(file));

    /// если файл не существует, создаёт файл с папками из его пути и закрывает этот фвйл
    public static void Create(string file)
    {
        file = Path.FixSeparators(file);
        if (!Exists(file))
        {
            if (file.Contains(Path.Sep))
                Directory.Create(file.Remove(file.LastIndexOf(Path.Sep)));
            using System.IO.FileStream stream = System.IO.File.Create(file);
            stream.Close();
        }
    }

    public static void Copy(string srcPath, string newPath, bool overwrite = false)
    {
        srcPath = Path.FixSeparators(srcPath);
        newPath = Path.FixSeparators(newPath);
        if (!overwrite && Exists(newPath))
            throw new Exception($"file <{newPath}> alredy exists");
        System.IO.File.Copy(srcPath, newPath, overwrite);
    }

    public static void Delete(string file) => System.IO.File.Delete(Path.FixSeparators(file));

    public static byte[] ReadAllBytes(string file)
    {
        file = Path.FixSeparators(file);
        using System.IO.FileStream stream = OpenRead(file);
        int size = GetSize(file).ToInt();
        byte[] output = new byte[size];
        if (stream.Read(output, 0, size) < size)
            throw new Exception("can't read all bytes");
        stream.Close();
        return output;
    }

    public static string ReadAllText(string file) => ReadAllBytes(file).BytesToString(StringConverter.UTF8);

    public static void WriteAllBytes(string file, byte[] content)
    {
        file = Path.FixSeparators(file);
        using System.IO.FileStream stream = OpenWrite(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void WriteAllText(string file, string content) => WriteAllBytes(file, content.ToBytes(StringConverter.UTF8));

    public static void AppendAllBytes(string file, byte[] content)
    {
        file = Path.FixSeparators(file);
        using System.IO.FileStream stream = OpenAppend(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void AppendAllText(string file, string content) => AppendAllBytes(file, content.ToBytes(StringConverter.UTF8));

    public static System.IO.FileStream OpenRead(string file)
    {
        file = Path.FixSeparators(file);
        if (Exists(file))
            return System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                System.IO.FileShare.ReadWrite);
        throw new Exception($"file not found: <{file}>");
    }

    public static System.IO.FileStream OpenWrite(string file)
    {
        file = Path.FixSeparators(file);
        if (Exists(file))
            Delete(file);
        Create(file);
        return System.IO.File.Open(file, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
    }
    public static System.IO.FileStream OpenAppend(string file)
    {
        file = Path.FixSeparators(file);
        Create(file);
        return System.IO.File.Open(file, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
    }

    public static void CreateSymlink(string sourceName, string symlinkName)
    {
        sourceName = Path.FixSeparators(sourceName);
        symlinkName = Path.FixSeparators(symlinkName);
        if (symlinkName.Contains(Path.Sep))
            Directory.Create(symlinkName.Remove(symlinkName.LastIndexOf(Path.Sep)));
        if (!Symlink.CreateSymbolicLink(symlinkName, sourceName, Symlink.SymlinkTarget.File))
            throw new InvalidOperationException($"some error occured while creating symlink\nFile.CreateSymlink({symlinkName}, {sourceName})");
    }
}
