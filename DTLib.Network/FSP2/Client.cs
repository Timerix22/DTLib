using System.Net;

namespace DTLib.Network.FSP2;

public class Client : IDisposable
{
    private TCPSocketClient _tcp;

    public Client(IPEndPoint serverEndpoint)
    {
        _tcp = new TCPSocketClient(serverEndpoint);
    }
    
    public void Dispose()
    {
        _tcp.Dispose();
    }
}