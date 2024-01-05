using DTLib.Dtsod;

namespace DTLib.Network;

//
// передача файлов по сети
//
public class FSP
{
    Socket MainSocket { get; init; }
    public FSP(Socket _mainSocket) => MainSocket = _mainSocket;

    public uint BytesDownloaded;
    public uint BytesUploaded;
    public uint Filesize;

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

    public void DownloadByManifest(IOPath dirOnServer, IOPath dirOnClient, bool overwrite = false, bool delete_excess = false)
    {
        var manifest = new DtsodV23(DownloadFileToMemory(dirOnServer + "manifest.dtsod").BytesToString(StringConverter.UTF8));
        var hasher = new Hasher();
        foreach (var fileOnServer in manifest.Keys)
        {
            IOPath fileOnClient = Path.Concat(dirOnClient, fileOnServer);
            if (!File.Exists(fileOnClient) || (overwrite && hasher.HashFile(fileOnClient).HashToString() != manifest[fileOnServer]))
                DownloadFile(Path.Concat(dirOnServer, fileOnServer), fileOnClient);
        }
        // удаление лишних файлов
        if (delete_excess)
        {
            foreach (var file in Directory.GetAllFiles(dirOnClient))
            {
                if (!manifest.ContainsKey(file.Str.Remove(0, dirOnClient.Length))) 
                    File.Delete(file);
            }
        }
    }

    public static void CreateManifest(IOPath dir)
    {
        if(!Directory.Exists(dir))
        {
            Directory.Create(dir);
            return;
        }
        
        StringBuilder manifestBuilder = new();
        Hasher hasher = new();
        if (Directory.GetFiles(dir).Contains(dir + "manifest.dtsod"))
            File.Delete(dir + "manifest.dtsod");
        foreach (var _file in Directory.GetAllFiles(dir))
        {
            var file = _file.Remove(0, dir.Length);
            manifestBuilder.Append(file);
            manifestBuilder.Append(": \"");
            byte[] hash = hasher.HashFile(Path.Concat(dir, file));
            manifestBuilder.Append(hash.HashToString());
            manifestBuilder.Append("\";\n");
        }
        File.WriteAllText(dir + "manifest.dtsod", manifestBuilder.ToString());
    }
}
