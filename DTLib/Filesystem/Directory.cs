namespace DTLib.Filesystem;

public static class Directory
{
    public static bool Exists(string dir) => System.IO.Directory.Exists(dir);

    // создает папку, если её не существует
    public static void Create(string dir)
    {
        if (!Exists(dir))
        {
            // проверяет существование папки, в которой нужно создать dir
            if (dir.Contains(Путь.Разд) && !Exists(dir.Remove(dir.LastIndexOf(Путь.Разд))))
                Create(dir.Remove(dir.LastIndexOf(Путь.Разд)));
            System.IO.Directory.CreateDirectory(dir);
        }
    }
    // копирует все файлы и папки
    public static void Copy(string source_dir, string new_dir, bool owerwrite = false)
    {
        Create(new_dir);
        var subdirs = new List<string>();
        List<string> files = GetAllFiles(source_dir, ref subdirs);
        for (int i = 0; i < subdirs.Count; i++)
            Create(subdirs[i].Replace(source_dir, new_dir));
        for (int i = 0; i < files.Count; i++)
            File.Copy(files[i], files[i].Replace(source_dir, new_dir), owerwrite);
    }

    // копирует все файлы и папки и выдаёт список конфликтующих файлов
    public static void Copy(string source_dir, string new_dir, out List<string> conflicts, bool owerwrite = false)
    {
        conflicts = new List<string>();
        var subdirs = new List<string>();
        List<string> files = GetAllFiles(source_dir, ref subdirs);
        Create(new_dir);
        for (int i = 0; i < subdirs.Count; i++)
            Create(subdirs[i].Replace(source_dir, new_dir));
        for (int i = 0; i < files.Count; i++)
        {
            string newfile = files[i].Replace(source_dir, new_dir);
            if (File.Exists(newfile))
                conflicts.Add(newfile);
            File.Copy(files[i], newfile, owerwrite);
        }
    }

    // удаляет папку со всеми подпапками и файлами
    public static void Delete(string dir)
    {
        var subdirs = new List<string>();
        List<string> files = GetAllFiles(dir, ref subdirs);
        for (int i = 0; i < files.Count; i++)
            File.Delete(files[i]);
        for (int i = subdirs.Count - 1; i >= 0; i--)
        {
            Log($"deleting {subdirs[i]}");
            if (Exists(subdirs[i]))
                System.IO.Directory.Delete(subdirs[i], true);
        }
        Log($"deleting {dir}");
        if (Exists(dir))
            System.IO.Directory.Delete(dir, true);
    }

    public static string[] GetFiles(string dir) => System.IO.Directory.GetFiles(dir);
    public static string[] GetFiles(string dir, string searchPattern) => System.IO.Directory.GetFiles(dir, searchPattern);
    public static string[] GetDirectories(string dir) => System.IO.Directory.GetDirectories(dir);

    // выдает список всех файлов
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

    // выдает список всех файлов и подпапок в папке
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

    public static void CreateSymlink(string sourceName, string symlinkName)
    {
        if (symlinkName.Contains(Путь.Разд))
            Create(symlinkName.Remove(symlinkName.LastIndexOf(Путь.Разд)));
        if (!Symlink.CreateSymbolicLink(symlinkName, sourceName, Symlink.SymlinkTarget.Directory))
            throw new InvalidOperationException($"some error occured while creating symlink\nDirectory.CreateSymlink({symlinkName}, {sourceName})");
    }

    // copies directory with symlinks instead of files
    public static int SymCopy(string srcdir, string newdir)
    {
        List<string> files = GetAllFiles(srcdir);
        if (!srcdir.EndsWith(Путь.Разд)) srcdir += Путь.Разд;
        if (!newdir.EndsWith(Путь.Разд)) newdir += Путь.Разд;
        int i = 0;
        for (; i < files.Count; i++)
            File.CreateSymlink(files[i], files[i].Replace(srcdir, newdir));
        return i;
    }
}
