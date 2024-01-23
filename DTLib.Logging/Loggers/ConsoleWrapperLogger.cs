using DTLib.Logging;

namespace launcher_client;

public interface IConsoleWrapper : IDisposable
{
    public void WriteLine(string msg);
}

public class ConsoleWrapperLogger : ILogger
{
    public bool DebugLogEnabled { get; set; } = false;
    public bool InfoLogEnabled { get; set; } = true;
    public bool WarnLogEnabled { get; set; } = true;
    public bool ErrorLogEnabled { get; set; } = true;
    public ILogFormat Format { get; set; }

    private readonly IConsoleWrapper _consoleWrapper;
    
    public ConsoleWrapperLogger(IConsoleWrapper consoleWrapper, ILogFormat format)
    {
        _consoleWrapper = consoleWrapper;
        Format = format;
    }
    
    public ConsoleWrapperLogger(IConsoleWrapper consoleWrapper) 
        : this(consoleWrapper, new DefaultLogFormat()) 
    {}

    public void Log(string context, LogSeverity severity, object message, ILogFormat format)
    {
        if(!this.CheckSeverity(severity))
            return;
        
        var msg = format.CreateMessage(context, severity, message);
        lock (_consoleWrapper) 
            _consoleWrapper.WriteLine(msg);
    }
    
    public void Dispose()
    {
        _consoleWrapper.Dispose();
    }
    
    ~ConsoleWrapperLogger() => Dispose();
}