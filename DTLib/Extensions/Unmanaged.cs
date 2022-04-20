using System.Runtime.InteropServices;

namespace DTLib.Extensions;

public static class Unmanaged
{
    public static unsafe IntPtr ToHGlobalUTF8(this string s)
    {
        byte[] buf = s.ToBytes();
        int bl = buf.Length;
        byte* ptr=(byte*)Marshal.AllocHGlobal(bl + 1);
        for (int i = 0; i < bl; i++)
            ptr[i] = buf[i];
        ptr[bl] = (byte)'\0';
        return (IntPtr) ptr;
    }

    public static string ToStringUTF8(this IntPtr p) => Marshal.PtrToStringUTF8(p);
}