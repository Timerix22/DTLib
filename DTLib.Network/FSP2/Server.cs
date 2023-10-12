using System.Net;

namespace DTLib.Network.FSP2;

public class Server : IDisposable
{
    
    private TCPSocketServer _tcp;
    private readonly Func<Request, Response> _requestHandler;
    
    public Server(IPEndPoint localEndpoint, Func<Request, Response> requestHandler, Action<Exception> exceptionHandler)
    {
        _requestHandler = requestHandler;
        _tcp = new TCPSocketServer(localEndpoint, HandleFSP2Connection, exceptionHandler);
    }

    void HandleFSP2Connection(TCPSocketServer.ConnectionParams p)
    {
        
    }
    
    public void Dispose()
    {
        _tcp.Dispose();
    }
}