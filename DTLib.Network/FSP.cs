using DTLib.Dtsod;

namespace DTLib.Network;

//
// передача файлов по сети
//
public class FSP
{
    Socket MainSocket { get; init; }
    public FSP(Socket _mainSocket) => MainSocket = _mainSocket;

    public uint BytesDownloaded { get; private set; }
    public uint BytesUploaded { get; private set; }
    public uint Filesize { get; private set; }

    // скачивает файл с помощью FSP протокола
    public void DownloadFile(IOPath filePath_server, IOPath filePath_client)
    {
        filePath_server.ThrowIfEscapes();
        filePath_client.ThrowIfEscapes();
        lock (MainSocket)
        {
            MainSocket.SendPackage("requesting file download".ToBytes(StringConverter.UTF8));
            MainSocket.SendPackage(filePath_server.Str.ToBytes(StringConverter.UTF8));
        }
        DownloadFile(filePath_client);
    }

    public void DownloadFile(IOPath filePath_client)
    {
        filePath_client.ThrowIfEscapes();
        using System.IO.Stream fileStream = File.OpenWrite(filePath_client);
        Download_SharedCode(fileStream, true);
        fileStream.Close();
    }

    public byte[] DownloadFileToMemory(IOPath filePath_server)
    {
        filePath_server.ThrowIfEscapes();
        lock (MainSocket)
        {
            MainSocket.SendPackage("requesting file download".ToBytes(StringConverter.UTF8));
            MainSocket.SendPackage(filePath_server.Str.ToBytes(StringConverter.UTF8));
        }
        return DownloadFileToMemory();
    }

    public byte[] DownloadFileToMemory()
    {
        using var fileStream = new System.IO.MemoryStream();
        Download_SharedCode(fileStream, false);
        byte[] output = fileStream.ToArray();
        fileStream.Close();
        return output;
    }

    private void Download_SharedCode(System.IO.Stream fileStream, bool requiresFlushing)
    {
        lock (MainSocket)
        {
            BytesDownloaded = 0;
            Filesize = MainSocket.GetPackage().BytesToString(StringConverter.UTF8).ToUInt();
            MainSocket.SendPackage("ready".ToBytes(StringConverter.UTF8));
            int packagesCount = 0;
            byte[] buffer = new byte[5120];
            int fullPackagesCount = (int)(Filesize / buffer.Length);
            // получение полных пакетов файла
            for (byte n = 0; packagesCount < fullPackagesCount; packagesCount++)
            {
                buffer = MainSocket.GetPackage();
                BytesDownloaded += (uint)buffer.Length;
                fileStream.Write(buffer, 0, buffer.Length);
                if (requiresFlushing)
                {
                    if (n == 100)
                    {
                        fileStream.Flush();
                        n = 0;
                    }
                    else n++;
                }
            }
            // получение остатка
            if ((Filesize - fileStream.Position) > 0)
            {
                MainSocket.SendPackage("remain request".ToBytes(StringConverter.UTF8));
                buffer = MainSocket.GetPackage();
                BytesDownloaded += (uint)buffer.Length;
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
        if (requiresFlushing)
            fileStream.Flush();
    }

    // отдаёт файл с помощью FSP протокола
    public void UploadFile(IOPath filePath)
    {
        filePath.ThrowIfEscapes();
        BytesUploaded = 0;
        using System.IO.FileStream fileStream = File.OpenRead(filePath);
        Filesize = File.GetSize(filePath).ToUInt();
        lock (MainSocket)
        {
            MainSocket.SendPackage(Filesize.ToString().ToBytes(StringConverter.UTF8));
            MainSocket.GetAnswer("ready");
            byte[] buffer = new byte[5120];
            int packagesCount = 0;
            int fullPackagesCount = (int)(Filesize / buffer.Length);
            // отправка полных пакетов файла
            for (; packagesCount < fullPackagesCount; packagesCount++)
            {
                fileStream.Read(buffer, 0, buffer.Length);
                MainSocket.SendPackage(buffer);
                BytesUploaded += (uint)buffer.Length;
            }
            // отправка остатка
            if ((Filesize - fileStream.Position) > 0)
            {
                MainSocket.GetAnswer("remain request");
                buffer = new byte[(Filesize - fileStream.Position).ToInt()];
                fileStream.Read(buffer, 0, buffer.Length);
                MainSocket.SendPackage(buffer);
                BytesUploaded += (uint)buffer.Length;
            }
        }
        fileStream.Close();
    }
}
