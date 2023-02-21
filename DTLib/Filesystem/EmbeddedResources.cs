using System.IO;
using System.Reflection;

namespace DTLib.Filesystem;

public static class EmbeddedResources
{
    public static Stream GetResourceStream(string resourcePath, Assembly assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        return assembly.GetManifestResourceStream(resourcePath) 
               ?? throw new Exception($"embedded resource <{resourcePath}> not found in assembly {assembly.FullName}");
    }

    public static byte[] ReadBynary(string resourcePath, Assembly assembly = null)
    {
        // is required to get the Assembly called ReadBynary
        // otherwise Assembly.GetCallingAssembly() in GetResourceStream will get DTLib assembly always
        assembly ??= Assembly.GetCallingAssembly();
        using var reader = new BinaryReader(GetResourceStream(resourcePath, assembly));
        return reader.ReadBytes(int.MaxValue);
    }


    public static string ReadText(string resourcePath, Assembly assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        using var reader = new StreamReader(GetResourceStream(resourcePath, assembly));
        return reader.ReadToEnd();
    }

    public static void CopyToFile(string resourcePath, string filePath, Assembly assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        using var file = File.OpenWrite(filePath);
        GetResourceStream(resourcePath, assembly).FluentCopyTo(file).Close();
    }
}