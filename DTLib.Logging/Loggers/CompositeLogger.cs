namespace DTLib.Logging.New;

/// <summary>
/// This class can be used for unite many loggers into one
/// </summary>
public class CompositeLogger : ILogger
{
    public bool DebugLogEnabled { get; set; } = false;
    public bool InfoLogEnabled { get; set; } = true;
    public bool WarnLogEnabled { get; set; } = true;
    public bool ErrorLogenabled { get; set; } = true;
    public ILogFormat Format { get; }
    
    protected ILogger[] _loggers;

    public CompositeLogger(ILogFormat format, params ILogger[] loggers)
    {
        Format = format;
        _loggers = loggers;
    }
    
    public CompositeLogger(params ILogger[] loggers) : this(new DefaultLogFormat(), loggers) 
    {}
    
    
    public void Log(string context, LogSeverity severity, object message, ILogFormat format)
    {
        if(!this.CheckSeverity(severity))
            return;
        
        for (int i = 0; i < _loggers.Length; i++) 
            _loggers[i].Log(context, severity, message, format);
    }

    public void Log(string context, LogSeverity severity, object message)
        => Log(context, severity, message, Format);
    
    
    public void Dispose()
    {
        for (int i = 0; i < _loggers.Length; i++)
        {
            _loggers[i].Dispose();
        }
    }
}