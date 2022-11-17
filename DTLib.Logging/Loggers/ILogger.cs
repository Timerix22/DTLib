namespace DTLib.Logging.New;

public interface ILogger : IDisposable
{
    
    ILogFormat Format { get; }
    void Log(string context, LogSeverity severity, object message);
    void Log(string context, LogSeverity severity, object message, ILogFormat format);
}