using DTLib.Dtsod;
using System.Net.Sockets;

namespace DTLib.Network;

//
// передача файлов по сети
//
public class FSP
{
    Socket MainSocket { get; init; }
    public static bool debug = false;
    public FSP(Socket _mainSocket) => MainSocket = _mainSocket;

    public uint BytesDownloaded = 0;
    public uint BytesUploaded = 0;
    public uint Filesize = 0;

    // скачивает файл с помощью FSP протокола
    public void DownloadFile(string filePath_server, string filePath_client)
    {
        lock (MainSocket)
        {
            Debug("b", $"requesting file download: {filePath_server}\n");
            MainSocket.SendPackage("requesting file download".ToBytes());
            MainSocket.SendPackage(filePath_server.ToBytes());
        }
        DownloadFile(filePath_client);
    }

    public void DownloadFile(string filePath_client)
    {
        using System.IO.Stream fileStream = File.OpenWrite(filePath_client);
        Download_SharedCode(fileStream, true);
        fileStream.Close();
        Debug("g", $"   downloaded {BytesDownloaded} of {Filesize} bytes\n");
    }

    public byte[] DownloadFileToMemory(string filePath_server)
    {
        lock (MainSocket)
        {
            Debug("b", $"requesting file download: {filePath_server}\n");
            MainSocket.SendPackage("requesting file download".ToBytes());
            MainSocket.SendPackage(filePath_server.ToBytes());
        }
        return DownloadFileToMemory();
    }

    public byte[] DownloadFileToMemory()
    {
        using var fileStream = new System.IO.MemoryStream();
        Download_SharedCode(fileStream, false);
        byte[] output = fileStream.GetBuffer();
        fileStream.Close();
        Debug("g", $"   downloaded {BytesDownloaded} of {Filesize} bytes\n");
        return output;
    }

    void Download_SharedCode(System.IO.Stream fileStream, bool requiresFlushing)
    {
        lock (MainSocket)
        {
            BytesDownloaded = 0;
            Filesize = MainSocket.GetPackage().BytesToString().ToUInt();
            MainSocket.SendPackage("ready".ToBytes());
            int packagesCount = 0;
            byte[] buffer = new byte[5120];
            int fullPackagesCount = (Filesize / buffer.Length).Truncate();
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
                    else
                        n++;
                }
            }
            // получение остатка
            if ((Filesize - fileStream.Position) > 0)
            {
                MainSocket.SendPackage("remain request".ToBytes());
                buffer = MainSocket.GetPackage();
                BytesDownloaded += (uint)buffer.Length;
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
        if (requiresFlushing)
            fileStream.Flush();
    }

    // отдаёт файл с помощью FSP протокола
    public void UploadFile(string filePath)
    {
        BytesUploaded = 0;
        Debug("b", $"uploading file {filePath}\n");
        using System.IO.FileStream fileStream = File.OpenRead(filePath);
        Filesize = File.GetSize(filePath).ToUInt();
        lock (MainSocket)
        {
            MainSocket.SendPackage(Filesize.ToString().ToBytes());
            MainSocket.GetAnswer("ready");
            byte[] buffer = new byte[5120];
            int packagesCount = 0;
            int fullPackagesCount = (Filesize / buffer.Length).Truncate();
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
        Debug("g", $"   uploaded {BytesUploaded} of {Filesize} bytes\n");
    }

    public void DownloadByManifest(string dirOnServer, string dirOnClient, bool overwrite = false, bool delete_excess = false)
    {
        if (!dirOnClient.EndsWith("\\"))
            dirOnClient += "\\";
        if (!dirOnServer.EndsWith("\\"))
            dirOnServer += "\\";
        Debug("b", "downloading manifest <", "c", dirOnServer + "manifest.dtsod", "b", ">\n");
        var manifest = new DtsodV22(DownloadFileToMemory(dirOnServer + "manifest.dtsod").BytesToString());
        Debug("g", $"found {manifest.Values.Count} files in manifest\n");
        var hasher = new Hasher();
        foreach (string fileOnServer in manifest.Keys)
        {
            string fileOnClient = dirOnClient + fileOnServer;
            Debug("b", "file <", "c", fileOnClient, "b", ">...  ");
            if (!File.Exists(fileOnClient))
            {
                DebugNoTime("y", "doesn't exist\n");
                DownloadFile(dirOnServer + fileOnServer, fileOnClient);
            }
            else if (overwrite && hasher.HashFile(fileOnClient).HashToString() != manifest[fileOnServer])
            {
                DebugNoTime("y", "outdated\n");
                DownloadFile(dirOnServer + fileOnServer, fileOnClient);
            }
            else
                DebugNoTime("g", "without changes\n");
        }
        // удаление лишних файлов
        if (delete_excess)
        {
            foreach (string file in Directory.GetAllFiles(dirOnClient))
            {
                if (!manifest.ContainsKey(file.Remove(0, dirOnClient.Length)))
                {
                    Debug("y", $"deleting excess file: {file}\n");
                    File.Delete(file);
                }
            }
        }
    }

    public static void CreateManifest(string dir)
    {
        if (!dir.EndsWith("\\"))
            dir += "\\";
        Log($"b", $"creating manifest of {dir}\n");
        StringBuilder manifestBuilder = new();
        Hasher hasher = new();
        if (Directory.GetFiles(dir).Contains(dir + "manifest.dtsod"))
            File.Delete(dir + "manifest.dtsod");
        foreach (string _file in Directory.GetAllFiles(dir))
        {
            string file = _file.Remove(0, dir.Length);
            manifestBuilder.Append(file);
            manifestBuilder.Append(": \"");
            byte[] hash = hasher.HashFile(dir + file);
            manifestBuilder.Append(hash.HashToString());
            manifestBuilder.Append("\";\n");
        }
        Debug($"g", $"   manifest of {dir} created\n");
        File.WriteAllText(dir + "manifest.dtsod", manifestBuilder.ToString());
    }

    static void Debug(params string[] msg)
    {
        if (debug) Log(msg);
    }
    static void DebugNoTime(params string[] msg)
    {
        if (debug) LogNoTime(msg);
    }
}
