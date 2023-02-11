namespace DTLib.Logging.New;

public interface ILogger : IDisposable
{
    
    ILogFormat Format { get; }
    bool DebugLogEnabled { get; set; }
    bool InfoLogEnabled { get; set; }
    bool WarnLogEnabled { get; set; }
    bool ErrorLogEnabled { get; set; }
    
    void Log(string context, LogSeverity severity, object message, ILogFormat format);
    
}