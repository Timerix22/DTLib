using System.Runtime.InteropServices;

namespace DTLib;

public static class DependencyResolver
{
    private static bool DepsCopied=false;
    
    public static void CopyLibs()
    {
        if(DepsCopied) return;
        string depsdir = $"Dependencies{Path.Sep}";
        depsdir += Environment.OSVersion.Platform switch
        {
            PlatformID.Unix => "linux",
            PlatformID.Win32NT => "windows",
            _=> throw new Exception($"unsupported os {Environment.OSVersion.Platform}")
        };
        depsdir += Path.Sep;
        depsdir += RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture.Arm64 => "arm64",
            Architecture.Arm => "arm",
            _=> throw new Exception($"unsupported platform {RuntimeInformation.ProcessArchitecture}")
        };
        foreach (var file in Directory.GetAllFiles(depsdir))
        {
            var extracted = file.Substring(file.LastIndexOf(Path.Sep) + 1);
            File.Copy(file,extracted, true);
            Log("g",$"{extracted} copied");
        }
        DepsCopied = true;
    }
}