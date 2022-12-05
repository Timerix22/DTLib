namespace DTLib.Logging.New;

/// <summary>
/// This class can be used for unite many loggers into one
/// </summary>
public class CompositeLogger : ILogger
{
    public bool DebugLogEnabled
    {
        get => _debugLogEnabled;
        set
        {
            _debugLogEnabled = value;
            for (int i = 0; i < _loggers.Length; i++)
                _loggers[i].DebugLogEnabled = value;
        }
    }

    public bool InfoLogEnabled
    {
        get => _infoLogEnabled;
        set
        {
            _infoLogEnabled = true;
            for (int i = 0; i < _loggers.Length; i++)
                _loggers[i].InfoLogEnabled = value;
        }
    }

    public bool WarnLogEnabled
    {
        get => _warnLogEnabled;
        set
        {
            _warnLogEnabled = value;
            for (int i = 0; i < _loggers.Length; i++)
                _loggers[i].WarnLogEnabled = value;
        }
    }

    public bool ErrorLogenabled
    {
        get => _errorLogenabled;
        set
        {
            _errorLogenabled = value;
            for (int i = 0; i < _loggers.Length; i++)
                _loggers[i].ErrorLogenabled = value;
        }
    }

    public ILogFormat Format { get; }
    
    protected ILogger[] _loggers;
    private bool _debugLogEnabled =
#if DEBUG
        true;
#else 
        false;
#endif
    private bool _infoLogEnabled = true;
    private bool _warnLogEnabled = true;
    private bool _errorLogenabled = true;

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

    public void Dispose()
    {
        for (int i = 0; i < _loggers.Length; i++) 
            _loggers[i].Dispose();
    }
}