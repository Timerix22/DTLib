using System.Collections.Generic;

namespace DTLib.Filesystem
{

    public static class Directory
    {
        public static bool Exists(string dir) => System.IO.Directory.Exists(dir);

        // создает папку, если её не существует
        public static void Create(string dir)
        {
            if (!Directory.Exists(dir))
            {
                // проверяет существование папки, в которой нужно создать dir
                if (dir.Contains("\\") && !Directory.Exists(dir.Remove(dir.LastIndexOf('\\'))))
                    Create(dir.Remove(dir.LastIndexOf('\\')));
                System.IO.Directory.CreateDirectory(dir);
            }
        }
        // копирует все файлы и папки
        public static void Copy(string source_dir, string new_dir, bool owerwrite = false)
        {
            Create(new_dir);
            List<string> subdirs = new List<string>();
            List<string> files = GetAllFiles(source_dir, ref subdirs);
            for (int i = 0; i < subdirs.Count; i++)
            {
                Create(subdirs[i].Replace(source_dir, new_dir));
            }
            for (int i = 0; i < files.Count; i++)
            {
                string f = files[i].Replace(source_dir, new_dir);
                File.Copy(files[i], f, owerwrite);
                //PublicLog.Log(new string[] {"g", $"file <", "c", files[i], "b", "> have copied to <", "c", newfile, "b", ">\n'" });
            }
        }

        // копирует все файлы и папки и выдаёт список конфликтующих файлов
        public static void Copy(string source_dir, string new_dir, out List<string> conflicts, bool owerwrite = false)
        {
            conflicts = new List<string>();
            var subdirs = new List<string>();
            var files = GetAllFiles(source_dir, ref subdirs);
            Create(new_dir);
            for (int i = 0; i < subdirs.Count; i++)
            {
                Create(subdirs[i].Replace(source_dir, new_dir));
            }
            for (int i = 0; i < files.Count; i++)
            {
                string newfile = files[i].Replace(source_dir, new_dir);
                if (File.Exists(newfile)) conflicts.Add(newfile);
                File.Copy(files[i], newfile, owerwrite);
                //PublicLog.Log(new string[] {"g", $"file <", "c", files[i], "b", "> have copied to <", "c", newfile, "b", ">\n'" });
            }
        }

        // удаляет папку со всеми подпапками и файлами
        public static void Delete(string dir)
        {
            var subdirs = new List<string>();
            var files = GetAllFiles(dir, ref subdirs);
            for (int i = 0; i < files.Count; i++)
                File.Delete(files[i]);
            for (int i = subdirs.Count - 1; i >= 0; i--)
                System.IO.Directory.Delete(subdirs[i]);
            System.IO.Directory.Delete(dir);
        }

        public static string[] GetFiles(string dir) => System.IO.Directory.GetFiles(dir);
        public static string[] GetFiles(string dir, string searchPattern) => System.IO.Directory.GetFiles(dir, searchPattern);
        public static string[] GetDirectories(string dir) => System.IO.Directory.GetDirectories(dir);

        // выдает список всех файлов
        public static List<string> GetAllFiles(string dir)
        {
            List<string> all_files = new List<string>();
            string[] cur_files = Directory.GetFiles(dir);
            for (int i = 0; i < cur_files.Length; i++)
            {
                all_files.Add(cur_files[i]);
                //PublicLog.Log(new string[] { "b", "file found: <", "c", cur_files[i], "b", ">\n" });
            }
            string[] cur_subdirs = Directory.GetDirectories(dir);
            for (int i = 0; i < cur_subdirs.Length; i++)
            {
                //PublicLog.Log(new string[] { "b", "subdir found: <", "c", cur_subdirs[i], "b", ">\n" });
                all_files.AddRange(GetAllFiles(cur_subdirs[i]));
            }
            return all_files;
        }

        // выдает список всех файлов и подпапок в папке
        public static List<string> GetAllFiles(string dir, ref List<string> all_subdirs)
        {
            List<string> all_files = new List<string>();
            string[] cur_files = Directory.GetFiles(dir);
            for (int i = 0; i < cur_files.Length; i++)
            {
                all_files.Add(cur_files[i]);
                //PublicLog.Log(new string[] { "b", "file found: <", "c", cur_files[i], "b", ">\n" });
            }
            string[] cur_subdirs = Directory.GetDirectories(dir);
            for (int i = 0; i < cur_subdirs.Length; i++)
            {
                all_subdirs.Add(cur_subdirs[i]);
                //PublicLog.Log(new string[] { "b", "subdir found: <", "c", cur_subdirs[i], "b", ">\n" });
                all_files.AddRange(GetAllFiles(cur_subdirs[i], ref all_subdirs));
            }
            return all_files;
        }

        public static string GetCurrent() => System.IO.Directory.GetCurrentDirectory();
    }
}
