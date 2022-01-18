using System.Runtime.InteropServices;

namespace DTLib.Filesystem;

internal class Symlink
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    internal static extern bool CreateSymbolicLink(string symlinkName, string sourceName, SymlinkTarget type);

    internal enum SymlinkTarget
    {
        File,
        Directory
    }
}
