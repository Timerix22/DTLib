namespace DTLib.Filesystem;

public static class File
{
    /// возвращает размер файла в байтах
    public static long GetSize(IOPath file) => new System.IO.FileInfo(file.Str).Length;

    public static bool Exists(IOPath file)
    {
        if (System.IO.File.Exists(file.Str)) return true;
        return false;
    }

    /// если файл не существует, создаёт файл с папками из его пути и закрывает этот фвйл
    public static void Create(IOPath file)
    {
        if (Exists(file)) return;
        
        Directory.Create(file.ParentDir());
        using System.IO.FileStream stream = System.IO.File.Create(file.Str);
        stream.Close();
    }

    public static void Copy(IOPath srcPath, IOPath newPath, bool overwrite)
    {
        if (Exists(newPath))
        {
            if(overwrite) System.IO.File.Delete(newPath.Str);
            else throw new Exception($"file <{newPath}> alredy exists");
        }
        else Directory.Create(newPath.ParentDir());
        using var srcFile=System.IO.File.Open(srcPath.Str, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        using var newFile=System.IO.File.Open(newPath.Str, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
        srcFile.CopyTo(newFile);
        srcFile.Close();
        newFile.Flush();
        newFile.Close();
    }

    public static void Move(IOPath current_path, IOPath target_path, bool overwrite)
    {
        if (Exists(target_path))
        {
            if (overwrite)
                Delete(target_path);
            else throw new Exception($"file {target_path} already exists");
        }
        System.IO.File.Move(current_path.Str, target_path.Str);
    }

    public static void Delete(IOPath file) => System.IO.File.Delete(file.Str);

    public static byte[] ReadAllBytes(IOPath file)
    {
        
        using System.IO.FileStream stream = OpenRead(file);
        int size = GetSize(file).ToInt();
        byte[] output = new byte[size];
        if (stream.Read(output, 0, size) < size)
            throw new Exception("can't read all bytes");
        stream.Close();
        return output;
    }

    public static string ReadAllText(IOPath file) => ReadAllBytes(file).BytesToString(StringConverter.UTF8);

    public static void WriteAllBytes(IOPath file, byte[] content)
    {
        using System.IO.FileStream stream = OpenWrite(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void WriteAllText(IOPath file, string content) => WriteAllBytes(file, content.ToBytes(StringConverter.UTF8));

    public static void AppendAllBytes(IOPath file, byte[] content)
    {
        using System.IO.FileStream stream = OpenAppend(file);
        stream.Write(content, 0, content.Length);
        stream.Close();
    }

    public static void AppendAllText(IOPath file, string content) => AppendAllBytes(file, content.ToBytes(StringConverter.UTF8));

    public static System.IO.FileStream OpenRead(IOPath file)
    {
        if (!Exists(file)) 
            throw new Exception($"file not found: <{file}>");
        return System.IO.File.Open(file.Str, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
    }

    public static System.IO.FileStream OpenWrite(IOPath file)
    {
        if (Exists(file))
            Delete(file);
        Create(file);
        return System.IO.File.Open(file.Str, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
    }
    public static System.IO.FileStream OpenAppend(IOPath file)
    {
        
        Create(file);
        return System.IO.File.Open(file.Str, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
    }

    public static void CreateSymlink(IOPath sourcePath, IOPath symlinkPath)
    {
        if (symlinkPath.Contains(Path.Sep))
            Directory.Create(symlinkPath.ParentDir());
        if (!Symlink.CreateSymbolicLink(symlinkPath.Str, sourcePath.Str, Symlink.SymlinkTarget.File))
            throw new InvalidOperationException($"some error occured while creating symlink\nFile.CreateSymlink({symlinkPath}, {sourcePath})");
    }
}
