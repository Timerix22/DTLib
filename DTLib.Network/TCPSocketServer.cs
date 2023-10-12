using System.Net;

namespace DTLib.Network;

/// <summary>
/// Multi-threaded TCP
/// </summary>
public class TCPSocketServer : IDisposable
{
    public readonly IPEndPoint LocalEndPoint;
    
    public record ConnectionParams(Socket ConnectionSocket, Thread ConnectionThread);
    
    protected Action<ConnectionParams>? _connectionHandler;
    protected Action<Exception> _exceptionCallback;
    protected Socket? _mainSocket;

    /// <param name="localEndpoint">address and port of the server in the local network</param>
    /// <param name="incomingConnectionHandler">this delegate is called on every incoming socket connection</param>
    /// <param name="exceptionCallback">this delegate is called on every Exception throwed in threads created by the server</param>
    public TCPSocketServer(IPEndPoint localEndpoint,
        Action<ConnectionParams>? incomingConnectionHandler,
        Action<Exception> exceptionCallback)
    {
        LocalEndPoint = localEndpoint;
        _connectionHandler = incomingConnectionHandler;
        _exceptionCallback = exceptionCallback;
    }

    private volatile int acceptLoopRunning = 0;
    
    /// <summary>
    /// Starts server on a new thread
    /// </summary>
    /// <exception cref="Exception">the server is already started</exception>
    public virtual void Listen()
    {
        if (Interlocked.CompareExchange(ref acceptLoopRunning, 1, 1) == 1)
            throw new Exception("Server is already started. Stop it to ");
        
        _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _mainSocket.Bind(LocalEndPoint);
        _mainSocket.Listen(64);
        Thread loopThread = new Thread(AcceptLoop);
        loopThread.Start();
    }

    /// server runs this loop in special thread
    private void AcceptLoop()
    {
        try
        {
            // atomic (thread-safe) comparison acceptLoopRunning==1
            while (Interlocked.CompareExchange(ref acceptLoopRunning, 1, 1) == 1)
            {
                Socket incomingConnection = _mainSocket!.Accept();
                Thread connectionThread = new Thread(HandleConnection);
                connectionThread.Start(new ConnectionParams(incomingConnection, connectionThread));
            }
        }
        catch (Exception e)
        {
            _exceptionCallback(e); 
            Stop();
        }
    }


    private void HandleConnection(object? _p)
    {
        var p = (ConnectionParams)_p!;
        try
        {
            _connectionHandler?.Invoke(p);
        }
        catch (Exception e)
        {
            _exceptionCallback(e);
        }
        finally
        {
            p.ConnectionSocket.Shutdown(SocketShutdown.Both);
            p.ConnectionSocket.Close();
        }
    }
    
    public virtual void Stop()
    {
        // atomic (thread-safe) comparison acceptLoopRunning==0 and assignement acceptLoopRunning=0
        if (Interlocked.CompareExchange(ref acceptLoopRunning, 0, 1) == 0)
            throw new Exception("Server isn't running");
        
        if(_mainSocket is null)
            return;
        
        _mainSocket.Shutdown(SocketShutdown.Both);
        _mainSocket.Close();
    }
    
    public void Dispose() => Stop();
}