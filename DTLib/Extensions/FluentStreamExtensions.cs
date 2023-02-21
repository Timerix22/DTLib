using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DTLib.Extensions;

public static class FluentStreamExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this Stream s, byte[] buff) 
        => s.Write(buff, 0, buff.Length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream FluentWriteByte(this Stream s, byte b)
    {
        s.WriteByte(b);
        return s;
    }
    
#region FluentWrite
/*************************************
            FluentWrite
*************************************/
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream FluentWrite(this Stream s, byte[] buff)
    {
        s.Write(buff);
        return s;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentWriteAsync(this Stream st, byte[] buff)
        => FluentWriteAsync(st, buff, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Stream> FluentWriteAsync(this Stream s, byte[] buff, CancellationToken ct)
    {
        await s.WriteAsync(buff, 0, buff.Length, ct);
        return s;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentWriteAsync(this Task<Stream> st, byte[] buff)
        => FluentWriteAsync(st, buff, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Stream> FluentWriteAsync(this Task<Stream> st, byte[] buff, CancellationToken ct)
    {
        Stream s = await st;
        await s.WriteAsync(buff, 0, buff.Length, ct);
        return s;
    }
    
#endregion
    
#region FluentCopy
/*************************************
            FluentCopy
*************************************/
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream FluentCopyTo(this Stream src, Stream dest)
    {
        src.CopyTo(dest);
        return src;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentCopyToAsync(this Stream src, Stream dest)
        => FluentCopyToAsync(src, dest, 81920, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentCopyToAsync(this Stream src, Stream dest, int bufferSize)
        => FluentCopyToAsync(src, dest, bufferSize, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Stream> FluentCopyToAsync(this Stream src, Stream dest, int bufferSize, CancellationToken ct)
    {
        await src.CopyToAsync(dest, bufferSize, ct);
        return src;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentCopyToAsync(this Task<Stream> src, Stream dest)
        => FluentCopyToAsync(src, dest, 81920, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentCopyToAsync(this Task<Stream> src, Stream dest, int bufferSize)
        => FluentCopyToAsync(src, dest, bufferSize, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Stream> FluentCopyToAsync(this Task<Stream> srcTask, Stream dest, int bufferSize, CancellationToken ct)
    {
        Stream src = await srcTask;
        await src.CopyToAsync(dest, bufferSize, ct);
        return src;
    }
    
#endregion
    
#region FluentFlush
/*************************************
            FluentFlush
*************************************/
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream FluentFlush(this Stream s)
    {
        s.Flush();
        return s;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentFlushAsync(this Stream s)
        => FluentFlushAsync(s, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Stream> FluentFlushAsync(this Stream s, CancellationToken ct)
    {
        await s.FlushAsync(ct);
        return s;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Stream> FluentFlushAsync(this Task<Stream> st)
        => FluentFlushAsync(st, CancellationToken.None);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Stream> FluentFlushAsync(this Task<Stream> st, CancellationToken ct)
    {
        Stream s = await st;
        await s.FlushAsync(ct);
        return s;
    }
    
#endregion

#region WriteString

/*************************************
            WriteString
*************************************/
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteString(this Stream stream, string s) => 
        stream.Write(s.ToBytes());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteString(this Stream stream, string s, Encoding enc) =>
        stream.Write(s.ToBytes(enc));
    
#endregion
    
#region FluentWriteString

/*************************************
            FluentWriteString
*************************************/
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream FluentWriteString(this Stream stream, string s) => 
        stream.FluentWrite(s.ToBytes());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream FluentWriteString(this Stream stream, string s, Encoding enc) =>
        stream.FluentWrite(s.ToBytes(enc));
    
#endregion
}