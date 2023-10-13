using System.Net;

namespace DTLib.Network;

public class TCPSocketClient : IDisposable
{
    protected Socket? _mainSocket;
    public readonly IPEndPoint ServerEndpoint;
    
    public TCPSocketClient(IPEndPoint serverEndpoint)
    {
        ServerEndpoint = serverEndpoint;
    }

    public virtual void Connect()
    {
        _mainSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _mainSocket.Connect(ServerEndpoint);
    }

    public virtual void Stop()
    {
        if(_mainSocket is null)
            return;
        _mainSocket.Shutdown(SocketShutdown.Both);
        _mainSocket.Close();
        _mainSocket = null;
    }

    public void Dispose() => Stop();

    public void Send(ref byte[] buffer)
    {
        if (_mainSocket is null)
            throw new NullReferenceException("TCP socket is null! You have to call Connect() befure calling Send()");
        _mainSocket.Send(buffer);
    }

    public int Recieve(ref byte[] buffer)
    {
        if (_mainSocket is null)
            throw new NullReferenceException("TCP socket is null! You have to call Connect() befure calling Recieve()");
        return _mainSocket.Receive(buffer);
    }
}