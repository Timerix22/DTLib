namespace DTLib.Logging.New;

/// wrapper around ILogger and LoggerExtensions that stores context
public class ContextLogger : ILogger
{
    public ILogger ParentLogger;
    public readonly string Context;

    public ContextLogger(string context,ILogger parentLogger)
    {
        ParentLogger = parentLogger;
        Context = context;
    }

    /// Appends subContext to Context
    public void Log(string subContext, LogSeverity severity, object message, ILogFormat format)
    {
        ParentLogger.Log($"{Context}/{subContext}", severity, message, format);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Log(LogSeverity severity, object message)
        => ParentLogger.Log(Context, severity, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogDebug(object message)
        => ParentLogger.LogDebug(Context, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogInfo(object message)
        => ParentLogger.LogInfo(Context, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogWarn(object message)
        => ParentLogger.LogWarn(Context, message);
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogWarn(Exception ex)
        => ParentLogger.LogWarn(Context, ex);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(object message)
        => ParentLogger.LogError(Context, message);
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(Exception ex)
        => ParentLogger.LogError(Context, ex);

    public void Dispose() => ParentLogger.Dispose();

    public ILogFormat Format => ParentLogger.Format;

    public bool DebugLogEnabled
    {
        get => ParentLogger.DebugLogEnabled;
        set => ParentLogger.DebugLogEnabled = value;
    }

    public bool InfoLogEnabled
    {
        get => ParentLogger.InfoLogEnabled;
        set => ParentLogger.InfoLogEnabled = value;
    }

    public bool WarnLogEnabled
    {
        get => ParentLogger.WarnLogEnabled;
        set => ParentLogger.WarnLogEnabled = value;
    }

    public bool ErrorLogEnabled
    {
        get => ParentLogger.ErrorLogEnabled;
        set => ParentLogger.ErrorLogEnabled = value;
    }
}