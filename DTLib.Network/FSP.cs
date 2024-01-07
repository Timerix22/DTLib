namespace DTLib.Network;

//
// передача файлов по сети
//
public class FSP
{
    public int BufferSize { get; init; } = 512 * 1024;
    
    readonly byte[] buffer;
    
    Socket MainSocket { get; init; }
    public FSP(Socket _mainSocket)
    {
        buffer = new byte[BufferSize];
        MainSocket = _mainSocket;
    }

    public long BytesDownloaded { get; private set; }
    public long BytesUploaded { get; private set; }
    public long Filesize { get; private set; }

    // скачивает файл с помощью FSP протокола
    public void DownloadFile(IOPath filePath_server, IOPath filePath_client)
    {
        filePath_server.ThrowIfEscapes();
        filePath_client.ThrowIfEscapes();
        lock (MainSocket)
        {
            MainSocket.SendPackage("requesting file download");
            MainSocket.SendPackage(filePath_server.Str);
        }
        DownloadFile(filePath_client);
    }

    public void DownloadFile(IOPath filePath_client)
    {
        filePath_client.ThrowIfEscapes();
        using System.IO.Stream fileStream = File.OpenWrite(filePath_client);
        DownloadToStream(fileStream);
    }

    public byte[] DownloadFileToMemory(IOPath filePath_server)
    {
        filePath_server.ThrowIfEscapes();
        lock (MainSocket)
        {
            MainSocket.SendPackage("requesting file download");
            MainSocket.SendPackage(filePath_server.Str);
        }
        return DownloadFileToMemory();
    }

    public byte[] DownloadFileToMemory()
    {
        using var fileStream = new System.IO.MemoryStream();
        DownloadToStream(fileStream);
        return fileStream.ToArray();
    }

    private static readonly byte[] fspHeaderStrBytes = "FSP_file_size:".ToBytes();
    public void DownloadToStream(System.IO.Stream fileStream)
    {
        lock (MainSocket)
        {
            BytesDownloaded = 0;
            var fspHeader = MainSocket.GetPackage();
            if (!fspHeader.StartsWith(fspHeaderStrBytes))
                throw new Exception(
                    $"invalid fsp header: '{fspHeader.BytesToString()}' " +
                    $"({fspHeader.MergeToString(',')})");
            Filesize = BitConverter.ToInt64(fspHeader, fspHeaderStrBytes.Length);
            if (Filesize < 0)
                throw new Exception("FileSize < 0");
            
            while (BytesDownloaded < Filesize)
            {
                int recievedCount = MainSocket.Receive(buffer);
                fileStream.Write(buffer, 0, recievedCount);
                BytesDownloaded += recievedCount;
            }

            if (BytesDownloaded != Filesize)
                throw new Exception($"expected {Filesize} bytes, but downloaded {BytesDownloaded} bytes");
        }
    }

    // отдаёт файл с помощью FSP протокола
    public void UploadFile(IOPath filePath)
    {
        filePath.ThrowIfEscapes();
        BytesUploaded = 0;
        using System.IO.FileStream fileStream = File.OpenRead(filePath);
        Filesize = fileStream.Length;
        lock (MainSocket)
        {
            byte[] filesizeBytes = BitConverter.GetBytes(Filesize);
            byte[] fspHeader = new byte[fspHeaderStrBytes.Length + filesizeBytes.Length];
            fspHeaderStrBytes.CopyTo(fspHeader, 0);
            filesizeBytes.CopyTo(fspHeader, fspHeaderStrBytes.Length);
            MainSocket.SendPackage(fspHeader);
            
            while (BytesUploaded < Filesize)
            {
                int readCount = fileStream.Read(buffer, 0, buffer.Length);
                MainSocket.Send(buffer, 0, readCount, SocketFlags.None);
                BytesUploaded += readCount;
            }
            
            if (BytesUploaded != Filesize)
                throw new Exception($"expected {Filesize} bytes, but uploaded {BytesDownloaded} bytes");
        }
    }
}
