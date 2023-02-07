namespace DTLib.Filesystem;

public static class Directory
{
    public static bool Exists(string dir, bool separatorsFixed)
    {
        if(!separatorsFixed)
            dir = Path.FixSeparators(dir);
        return System.IO.Directory.Exists(dir);
    }

    /// создает папку, если её не существует
    public static void Create(string dir, bool separatorsFixed)
    {
        if(!separatorsFixed) 
            dir = Path.FixSeparators(dir);
        if (!Exists(dir))
        {
            // проверяет существование папки, в которой нужно создать dir
            if (dir.Contains(Path.Sep))
            {
                string parentDir = dir.Remove(dir.LastIndexOf(Path.Sep));
                if(!Exists(parentDir,true))
                    Create(parentDir,true);
            }
            else System.IO.Directory.CreateDirectory(dir);
        }
    }
    /// копирует все файлы и папки
    public static void Copy(string source_dir, string new_dir, bool owerwrite)
    {
        Copy_internal(source_dir, new_dir, owerwrite, null);
    }

    /// копирует все файлы и папки и выдаёт список конфликтующих файлов
    public static void Copy(string source_dir, string new_dir, bool owerwrite, out List<string> conflicts)
    {
        conflicts = new List<string>();
        Copy_internal(source_dir, new_dir, owerwrite, conflicts);
    }

    private static void Copy_internal(string source_dir, string new_dir, bool owerwrite, List<string> conflicts)
    {
        bool countConflicts = conflicts is null;
        List<string> files = GetAllFiles(source_dir);
        Create(new_dir);
        for (int i = 0; i < files.Count; i++)
        {
            string newfile = Path.ReplaceBase(files[i], source_dir, new_dir);
            if (countConflicts && File.Exists(newfile))
                conflicts!.Add(newfile);
            File.Copy(files[i], newfile, owerwrite);
        }
    }

    /// удаляет папку со всеми подпапками и файлами
    public static void Delete(string dir, bool separatorsFixed)
    {
        if(!separatorsFixed)
            dir = Path.FixSeparators(dir);
        System.IO.Directory.Delete(dir, true);
    }

    public static string[] GetFiles(string dir, bool separatorsFixed)
    {
        if (!separatorsFixed)
            dir = Path.FixSeparators(dir);
        return System.IO.Directory.GetFiles(dir);
    }

    public static string[] GetFiles(string dir, string searchPattern, bool separatorsFixed)
    {
        if (!separatorsFixed)
            dir = Path.FixSeparators(dir);
        return System.IO.Directory.GetFiles(dir, searchPattern);
    }

    public static string[] GetDirectories(string dir, bool separatorsFixed)
    {
        if (!separatorsFixed)
            dir = Path.FixSeparators(dir);
        return System.IO.Directory.GetDirectories(dir);
    }

    /// выдает список всех файлов
    public static List<string> GetAllFiles(string dir)
    {
        var all_files = new List<string>();
        string[] cur_files = GetFiles(dir);
        for (int i = 0; i < cur_files.Length; i++)
            all_files.Add(cur_files[i]);
        string[] cur_subdirs = GetDirectories(dir);
        for (int i = 0; i < cur_subdirs.Length; i++)
            all_files.AddRange(GetAllFiles(cur_subdirs[i]));
        return all_files;
    }

    /// выдает список всех файлов и подпапок в папке
    public static List<string> GetAllFiles(string dir, ref List<string> all_subdirs)
    {
        var all_files = new List<string>();
        string[] cur_files = GetFiles(dir);
        for (int i = 0; i < cur_files.Length; i++)
            all_files.Add(cur_files[i]);
        string[] cur_subdirs = GetDirectories(dir);
        for (int i = 0; i < cur_subdirs.Length; i++)
        {
            all_subdirs.Add(cur_subdirs[i]);
            all_files.AddRange(GetAllFiles(cur_subdirs[i], ref all_subdirs));
        }
        return all_files;
    }

    public static string GetCurrent() => System.IO.Directory.GetCurrentDirectory();

    public static void CreateSymlink(string sourcePath, string symlinkPath, bool separatorsFixed)
    {
        if (!separatorsFixed)
        {
            sourcePath = Path.FixSeparators(sourcePath);
            symlinkPath = Path.FixSeparators(symlinkPath);
        }
        Create(Path.ParentDir(symlinkPath, true), true);
        if (!Symlink.CreateSymbolicLink(symlinkPath, sourcePath, Symlink.SymlinkTarget.Directory))
            throw new InvalidOperationException($"some error occured while creating symlink\nDirectory.CreateSymlink({symlinkPath}, {sourcePath})");
    }
}
