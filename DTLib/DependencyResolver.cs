using System.Runtime.InteropServices;

namespace DTLib;

public static class DependencyResolver
{
    private static object locker = new object();

    private static bool DepsCopied = false;

    public static void CopyLibs()
    {
        lock (locker)
        {
            if (DepsCopied) return;

            var os = Environment.OSVersion.Platform switch
            {
                PlatformID.Unix => "linux",
                PlatformID.Win32NT => "win",
                _ => throw new Exception($"unsupported os {Environment.OSVersion.Platform}")
            };
            var arch = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "x64",
                Architecture.X86 => "x86",
                Architecture.Arm64 => "arm64",
                Architecture.Arm => "arm",
                _ => throw new Exception($"unsupported platform {RuntimeInformation.ProcessArchitecture}")
            };

            string[] possibleLibDirs =
            {
                Path.Concat("runtimes", $"{os}"),
                Path.Concat("runtimes", $"{os}", "native"),
                Path.Concat("runtimes", $"{os}-{arch}"),
                Path.Concat("runtimes", $"{os}-{arch}", "native")
            };
            foreach (string dir in possibleLibDirs)
                if (Directory.Exists(dir))
                    foreach (var file in Directory.GetFiles(dir))
                    {
                        var extracted = file.Substring(file.LastIndexOf(Path.Sep) + 1);
                        File.Copy(file, extracted, true);
                        Log("g", $"{file} extracted");
                    }

            DepsCopied = true;
        }
    }
}