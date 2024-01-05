namespace DTLib.Network;

//
// отправка/получение пакетов
//
public static class Package
{
    // принимает пакет
    public static byte[] GetPackage(this Socket socket)
    {
        int ppackageLength = 0;
        // цикл выполняется пока не пройдёт 2000 мс
        for (ushort s = 0; s < 400; s += 1)
        {
            if (ppackageLength == 0 && socket.Available >= 4)
            {
                byte[] ppackageLengthBytes = new byte[4];
                socket.Receive(ppackageLengthBytes);
                ppackageLength = BitConverter.ToInt32(ppackageLengthBytes, 0);
            }
            else if (ppackageLength != 0 && socket.Available >= ppackageLength)
            {
                var data = new byte[ppackageLength];
                socket.Receive(data);
                return data;
            }
            else Thread.Sleep(5);
        }
        throw new Exception($"GetPackage() error: timeout. socket.Available={socket.Available}");
    }
    public static void GetPackage(this Socket socket, byte[] buffer)
    {
        int packageLength = 0;
        // цикл выполняется пока не пройдёт 2000 мс
        for (ushort s = 0; s < 400; s += 1)
        {
            if (packageLength == 0 && socket.Available >= 4)
            {
                byte[] ppackageLengthBytes = new byte[4];
                socket.Receive(ppackageLengthBytes);
                packageLength = BitConverter.ToInt32(ppackageLengthBytes, 0);
            }
            else if (packageLength != 0 && socket.Available >= packageLength)
            {
                if (buffer.Length < packageLength)
                    throw new Exception("buffer length is less than package length");
                socket.Receive(buffer);
            }
            else Thread.Sleep(5);
        }
        throw new Exception($"GetPackage() error: timeout. socket.Available={socket.Available}");
    }

    // отправляет пакет
    public static void SendPackage(this Socket socket, byte[] data) => SendPackage(socket, data, data.Length);
    public static void SendPackage(this Socket socket, byte[] buffer, int dataLength)
    {
        if (buffer.Length < dataLength)
            throw new Exception($"SendPackage() error: buffer is too small ({buffer.Length} bytes of {dataLength})");
        if (dataLength < 1)
            throw new Exception($"SendPackage() error: package has zero size");
        
        byte[] ppackageLength = BitConverter.GetBytes(buffer.Length);
        if (ppackageLength.Length != 4)
            throw new Exception("wrong with int to byte[] conversion");
        socket.Send(ppackageLength);
        socket.Send(buffer, 0, dataLength, SocketFlags.None);
    }
    
    public static void SendPackage(this Socket socket, string data) 
        => SendPackage(socket, data.ToBytes());

    // получает пакет и выбрасывает исключение, если пакет не соответствует образцу
    public static void GetAnswer(this Socket socket, string answer)
    {
        string rec = socket.GetPackage().BytesToString();
        if (rec != answer)
            throw new Exception($"GetAnswer() error: invalid answer: <{rec}>");
    }

    public static byte[] RequestPackage(this Socket socket, byte[] request)
    {
        socket.SendPackage(request);
        return socket.GetPackage();
    }
    public static byte[] RequestPackage(this Socket socket, string request) 
        => socket.RequestPackage(request.ToBytes());
}
