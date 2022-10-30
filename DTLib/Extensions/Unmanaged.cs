using System.Runtime.InteropServices;

namespace DTLib.Extensions;

public static class Unmanaged
{
    public static unsafe IntPtr StringToHGlobalUTF8(this string s)
    {
        byte[] buf = s.ToBytes();
        int bl = buf.Length;
        byte* ptr=(byte*)Marshal.AllocHGlobal(bl + 1);
        for (int i = 0; i < bl; i++)
            ptr[i] = buf[i];
        ptr[bl] = (byte)'\0';
        return (IntPtr) ptr;
    }

    public static unsafe int ASCII_length(IntPtr asciiStr)
    {
        byte* str = (byte*)asciiStr;
        while (*str != (byte)'\0')
            str++;
        long length = str - (byte*)asciiStr;
        if (length > Int32.MaxValue)
            throw new Exception($"ascii string length <{length}> > Int32.MaxValue");
        return (int)length;
    }

    public static unsafe string HGlobalUTF8ToString(this IntPtr p)
    {
        return StringConverter.UTF8.GetString((byte*)p, ASCII_length(p));
        /*if (p == IntPtr.Zero)
            throw new Exception("string pointer is null");
        int length = ASCII_length(p);
        byte[] buf = new byte[length];
        for (int i = 0; i < length; i++)
                buf[i] = ((byte*)p)[i];
        fixed(byte* bufptr=&buf[0])
        {
            return StringConverter.UTF8.GetString(bufptr, length);
        }*/
    }
}