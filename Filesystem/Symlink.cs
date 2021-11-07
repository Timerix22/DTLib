using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DTLib.Filesystem
{
    internal class Symlink
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static internal extern bool CreateSymbolicLink(string symlinkName, string sourceName, SymlinkTarget type);

        internal enum SymlinkTarget
        {
            File,
            Directory
        }
    }
}
