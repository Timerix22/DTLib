using DTLib.Filesystem;

namespace DTLib.Dtsod.V24;

public static class DependencyResolver
{
    private static bool KerepCopied=false;
    
    public static void CopyLibs()
    {
        if(KerepCopied) return;
        
        string kereplib = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? "kerep.dll"
            : "kerep.so";
        File.Copy($"Dependencies{Путь.Разд}{kereplib}",kereplib, true);
        KerepCopied = true;
        Log("g",$"{kereplib} copied");
    }
}