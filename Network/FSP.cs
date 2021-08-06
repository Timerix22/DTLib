using DTLib.Filesystem;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static DTLib.PublicLog;

namespace DTLib.Network
{
    //
    // передача файлов по сети
    //
    public class FSP
    {
        Socket mainSocket;
        public bool debug = true;
        public FSP(Socket _mainSocket) => mainSocket = _mainSocket;

        public uint BytesDownloaded = 0;
        public uint BytesUploaded = 0;
        public uint Filesize = 0;

        // скачивает файл с помощью FSP протокола
        public void DownloadFile(string filePath_server, string filePath_client)
        {
            if (debug) Log("b", $"requesting file download: {filePath_server}\n");
            mainSocket.SendPackage("requesting file download".ToBytes());
            mainSocket.SendPackage(filePath_server.ToBytes());
            DownloadFile(filePath_client);
        }

        public void DownloadFile(string filePath_client)
        {
            File.Create(filePath_client);
            using var fileStream = File.OpenWrite(filePath_client);
            Filesize = mainSocket.GetPackage().ToStr().ToUInt();
            var hashstr = mainSocket.GetPackage().HashToString();
            mainSocket.SendPackage("ready".ToBytes());
            int packagesCount = 0;
            byte[] buffer = new byte[5120];
            int fullPackagesCount = SimpleConverter.Truncate(Filesize / buffer.Length);
            // рассчёт скорости
            int seconds = 0;
            var speedCounter = new Timer(true, 1000, () =>
            {
                seconds++;
                Log("c", $"speed= {packagesCount * buffer.Length / (seconds * 1000)} kb/s\n");
            });
            // получение файла
            for (; packagesCount < fullPackagesCount; packagesCount++)
            {
                buffer = mainSocket.GetPackage();
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Flush();
            }
            speedCounter.Stop();
            // получение остатка
            if ((Filesize - fileStream.Position) > 0)
            {
                mainSocket.SendPackage("remain request".ToBytes());
                buffer = mainSocket.GetPackage();
                fileStream.Write(buffer, 0, buffer.Length);
            }
            fileStream.Flush();
            fileStream.Close();
            if (debug) Log(new string[] { "g", $"   downloaded {packagesCount * 5120 + buffer.Length} of {Filesize} bytes\n" });
        }
        public byte[] DownloadFileToMemory(string filePath_server)
        {
            if (debug) Log("b", $"requesting file download: {filePath_server}\n");
            mainSocket.SendPackage("requesting file download".ToBytes());
            mainSocket.SendPackage(filePath_server.ToBytes());
            using var fileStream = new System.IO.MemoryStream();
            var fileSize = mainSocket.GetPackage().ToStr().ToUInt();
            var hashstr = mainSocket.GetPackage().HashToString();
            mainSocket.SendPackage("ready".ToBytes());
            int packagesCount = 0;
            byte[] buffer = new byte[5120];
            int fullPackagesCount = SimpleConverter.Truncate(fileSize / buffer.Length);
            // рассчёт скорости
            int seconds = 0;
            var speedCounter = new Timer(true, 1000, () =>
            {
                seconds++;
                Log("c", $"speed= {packagesCount * buffer.Length / (seconds * 1000)} kb/s\n");
            });
            // получение файла
            for (; packagesCount < fullPackagesCount; packagesCount++)
            {
                buffer = mainSocket.GetPackage();
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Flush();
            }
            speedCounter.Stop();
            // получение остатка
            if ((fileSize - fileStream.Position) > 0)
            {
                mainSocket.SendPackage("remain request".ToBytes());
                buffer = mainSocket.GetPackage();
                fileStream.Write(buffer, 0, buffer.Length);
            }
            byte[] output = fileStream.GetBuffer();
            fileStream.Close();
            if (debug) Log(new string[] { "g", $"   downloaded {packagesCount * 5120 + buffer.Length} of {fileSize} bytes\n" });
            return output;
        }

        // отдаёт файл с помощью FSP протокола
        public void UploadFile(string filePath)
        {
            if (debug) Log("b", $"uploading file {filePath}\n");
            using var fileStream = File.OpenRead(filePath);
            Filesize = File.GetSize(filePath).ToUInt();
            var fileHash = new Hasher().HashFile(filePath);
            mainSocket.SendPackage(Filesize.ToString().ToBytes());
            mainSocket.SendPackage(fileHash);
            if (mainSocket.GetPackage().ToStr() != "ready") throw new Exception("user socket isn't ready");
            byte[] buffer = new byte[5120];
            var hashstr = fileHash.HashToString();
            int packagesCount = 0;
            // отправка файла
            int fullPackagesCount = SimpleConverter.Truncate(Filesize / buffer.Length);
            for (; packagesCount < fullPackagesCount; packagesCount++)
            {
                fileStream.Read(buffer, 0, buffer.Length);
                mainSocket.SendPackage(buffer);
            }
            // досылка остатка
            if ((Filesize - fileStream.Position) > 0)
            {
                if (mainSocket.GetPackage().ToStr() != "remain request") throw new Exception("FSP_Upload() error: didn't get remain request");
                buffer = new byte[(Filesize - fileStream.Position).ToInt()];
                fileStream.Read(buffer, 0, buffer.Length);
                mainSocket.SendPackage(buffer);
            }
            fileStream.Close();
            if (debug) Log(new string[] { "g", $"   uploaded {packagesCount * 5120 + buffer.Length} of {Filesize} bytes\n" });
        }

        public void DownloadByManifest(string dirOnServer, string dirOnClient, bool overwrite = false, bool delete_excess = false)
        {
            if (!dirOnClient.EndsWith("\\")) dirOnClient += "\\";
            if (!dirOnServer.EndsWith("\\")) dirOnServer += "\\";
            Log("b", "downloading manifest <", "c", dirOnServer + "manifest.dtsod", "b", ">\n");
            var manifest = new Dtsod(DownloadFileToMemory(dirOnServer + "manifest.dtsod").ToStr());
            Log("g", $"found {manifest.Values.Count} files in manifest\n");
            var hasher = new Hasher();
            foreach (string fileOnServer in manifest.Keys)
            {
                string fileOnClient = dirOnClient + fileOnServer;
                if (debug) Log("b", "file <", "c", fileOnClient, "b", ">...  ");
                if (!File.Exists(fileOnClient))
                {
                    if (debug) LogNoTime("y", "doesn't exist\n");
                    DownloadFile(dirOnServer + fileOnServer, fileOnClient);
                }
                else if (overwrite && hasher.HashFile(fileOnClient).HashToString() != manifest[fileOnServer])
                {
                    if (debug) LogNoTime("y", "outdated\n");
                    DownloadFile(dirOnServer + fileOnServer, fileOnClient);
                }
                else if (debug) LogNoTime("g", "without changes\n");
            }
            // удаление лишних файлов
            if (delete_excess)
            {
                List<string> dirs = new();
                foreach (string file in Directory.GetAllFiles(dirOnClient, ref dirs))
                {
                    if (!manifest.ContainsKey(file.Remove(0, dirOnClient.Length)))
                    {
                        if (debug) Log("y", $"deleting excess file: {file}\n");
                        File.Delete(file);
                    }
                }
                // удаление пустых папок
                foreach (string dir in dirs)
                {
                    if (Directory.Exists(dir) && Directory.GetAllFiles(dir).Count == 0)
                    {
                        if (debug) Log("y", $"deleting empty dir: {dir}\n");
                        Directory.Delete(dir);
                    }
                }
            }
        }

        static public void CreateManifest(string dir)
        {
            if (!dir.EndsWith("\\")) dir += "\\";
            Log($"b", $"creating manifest of {dir}\n");
            StringBuilder manifestBuilder = new();
            Hasher hasher = new();
            if (Directory.GetFiles(dir).Contains(dir + "manifest.dtsod")) File.Delete(dir + "manifest.dtsod");
            foreach (string _file in Directory.GetAllFiles(dir))
            {
                string file = _file.Remove(0, dir.Length);
                manifestBuilder.Append(file);
                manifestBuilder.Append(": \"");
                byte[] hash = hasher.HashFile(dir + file);
                manifestBuilder.Append(hash.HashToString());
                manifestBuilder.Append("\";\n");
            }
            File.WriteAllText(dir + "manifest.dtsod", manifestBuilder.ToString());
            Log($"g", $"   manifest of {dir} created\n");
        }
    }
}
