using System.IO;
using System.Reflection;

namespace DTLib.Filesystem;

public static class EmbeddedResources
{
    public static Stream GetResourceStream(string resourcePath, Assembly assembly = null) =>
        (assembly ?? Assembly.GetCallingAssembly())
        .GetManifestResourceStream(resourcePath)
        ?? throw new Exception($"embedded resource <{resourcePath}> not found");

    public static byte[] ReadBynary(string resourcePath, Assembly assembly = null)
    {
        if (assembly is null) assembly = Assembly.GetCallingAssembly();
        using var reader = new BinaryReader(GetResourceStream(resourcePath, assembly));
        return reader.ReadBytes(int.MaxValue);
    }


    public static string ReadText(string resourcePath, Assembly assembly = null)
    {
        if (assembly is null) assembly = Assembly.GetCallingAssembly();
        using var reader = new StreamReader(GetResourceStream(resourcePath, assembly));
        return reader.ReadToEnd();
    }
}