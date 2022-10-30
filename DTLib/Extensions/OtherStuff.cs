using System.IO;

namespace DTLib.Extensions;

public static class OtherStuff
{
    public static void Write(this Stream stream, byte[] buff)
        => stream.Write(buff, 0, buff.Length);
}