using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace DTLib.Network
{
    //
    // отправка/получение пакетов
    //
    static public class Package
    {
        // принимает пакет
        public static byte[] GetPackage(this Socket socket)
        {
            int packageSize = 0;
            byte[] data = new byte[2];
            // цикл выполняется пока не пройдёт 2000 мс
            for (ushort s = 0; s < 400; s += 1)
            {
                if (packageSize == 0 && socket.Available >= 2)
                {
                    socket.Receive(data, data.Length, 0);
                    packageSize = data.BytesToInt();

                }
                if (packageSize != 0 && socket.Available >= packageSize)
                {
                    data = new byte[packageSize];
                    socket.Receive(data, data.Length, 0);
                    return data;
                }
                else Thread.Sleep(5);
            }
            throw new Exception($"GetPackage() error: timeout. socket.Available={socket.Available}\n");
        }

        // отправляет пакет
        public static void SendPackage(this Socket socket, byte[] data)
        {
            if (data.Length > 65536) throw new Exception($"SendPackage() error: package is too big ({data.Length} bytes)");
            if (data.Length == 0) throw new Exception($"SendPackage() error: package has zero size");
            var list = new List<byte>();
            byte[] packageSize = data.Length.IntToBytes();
            if (packageSize.Length == 1) list.Add(0);
            list.AddRange(packageSize);
            list.AddRange(data);
            socket.Send(list.ToArray());
        }
    }
}
