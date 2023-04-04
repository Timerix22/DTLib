namespace DTLib.Filesystem;

public static class Directory
{
    public static bool Exists(IOPath dir) => System.IO.Directory.Exists(dir.Str);

    /// создает папку, если её не существует
    public static void Create(IOPath dir)
    {
        if (Exists(dir)) return;
        
        // creation of parent directories
        if (dir.Contains(Path.Sep))
        {
            var parentDir = dir.ParentDir(); 
            if(!Exists(parentDir))
                Create(parentDir);
        }
        
        System.IO.Directory.CreateDirectory(dir.Str);
    }
    /// копирует все файлы и папки
    public static void Copy(IOPath sourceDir, IOPath newDir, bool owerwrite)
    {
        Copy_internal(sourceDir, newDir, owerwrite, null);
    }

    /// копирует все файлы и папки и выдаёт список конфликтующих файлов
    public static void Copy(IOPath sourceDir, IOPath newDir, bool owerwrite, out List<IOPath> conflicts)
    {
        conflicts = new List<IOPath>();
        Copy_internal(sourceDir, newDir, owerwrite, conflicts);
    }

    private static void Copy_internal(IOPath sourceDir, IOPath newDir, bool owerwrite, List<IOPath> conflicts)
    {
        bool countConflicts = conflicts is not null;
        List<IOPath> files = GetAllFiles(sourceDir);
        Create(newDir);
        for (int i = 0; i < files.Count; i++)
        {
            var newfile = files[i].ReplaceBase(sourceDir, newDir);
            if (countConflicts && File.Exists(newfile))
                conflicts.Add(newfile);
            File.Copy(files[i], newfile, owerwrite);
        }
    }

    public static void Move(IOPath current_path, IOPath target_path, bool overwrite)
    {
        if (Exists(target_path))
        {
            if (overwrite)
                Delete(target_path);
            else throw new Exception($"directory {target_path} already exists");
        }
        System.IO.Directory.Move(current_path.Str, target_path.Str);
    }
    
    /// удаляет папку со всеми подпапками и файлами
    public static void Delete(IOPath dir) => 
        System.IO.Directory.Delete(dir.Str, true);

    public static IOPath[] GetFiles(IOPath dir) => 
        IOPath.ArrayCast(System.IO.Directory.GetFiles(dir.Str));

    public static IOPath[] GetFiles(IOPath dir, string searchPattern) => 
        IOPath.ArrayCast(System.IO.Directory.GetFiles(dir.Str, searchPattern));

    public static IOPath[] GetDirectories(IOPath dir) => 
        IOPath.ArrayCast(System.IO.Directory.GetDirectories(dir.Str));

    public static IOPath[] GetDirectories(IOPath dir, string searchPattern) => 
        IOPath.ArrayCast(System.IO.Directory.GetDirectories(dir.Str, searchPattern));

    /// выдает список всех файлов
    public static List<IOPath> GetAllFiles(IOPath dir)
    {
        return GetAllFiles_internal(dir, null);
    }

    /// выдает список всех файлов и подпапок в папке
    public static List<IOPath> GetAllFiles(IOPath dir, out List<IOPath> all_subdirs)
    {
        all_subdirs = new List<IOPath>();
        return GetAllFiles_internal(dir, all_subdirs);
    }
    private static List<IOPath> GetAllFiles_internal(IOPath dir, List<IOPath> all_subdirs)
    {
        bool rememberSubdirs = all_subdirs is not null;
        var all_files = new List<IOPath>();
        IOPath[] cur_files = GetFiles(dir);
        for (int i = 0; i < cur_files.Length; i++)
            all_files.Add(cur_files[i]);
        IOPath[] cur_subdirs = GetDirectories(dir);
        for (int i = 0; i < cur_subdirs.Length; i++)
        {
            if(rememberSubdirs)
                all_subdirs.Add(cur_subdirs[i]);
            all_files.AddRange(GetAllFiles_internal(cur_subdirs[i], all_subdirs));
        }
        return all_files;
    }

    public static string GetCurrent() => System.IO.Directory.GetCurrentDirectory();

    public static void CreateSymlink(string sourcePath, string symlinkPath)
    {
        if (symlinkPath.Contains(Path.Sep))
            Create(Path.ParentDir(symlinkPath));
        if (!Symlink.CreateSymbolicLink(symlinkPath, sourcePath, Symlink.SymlinkTarget.Directory))
            throw new InvalidOperationException($"some error occured while creating symlink\nDirectory.CreateSymlink({symlinkPath}, {sourcePath})");
    }
}
