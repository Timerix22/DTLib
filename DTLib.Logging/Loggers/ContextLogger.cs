namespace DTLib.Logging.New;

/// wrapper around ILogger and LoggerExtensions that stores context
public class ContextLogger : ILogger
{
    public ILogger Logger;
    public readonly string Context;

    public ContextLogger(ILogger logger, string context)
    {
        Logger = logger;
        Context = context;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Log(LogSeverity severity, object message)
        => Logger.Log(Context, severity, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogDebug(object message)
        => Logger.LogDebug(Context, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogInfo(object message)
        => Logger.LogInfo(Context, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogWarn(object message)
        => Logger.LogWarn(Context, message);
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogWarn(Exception ex)
        => Logger.LogWarn(Context, ex);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(object message)
        => Logger.LogError(Context, message);
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(Exception ex)
        => Logger.LogError(Context, ex);

    public void Dispose()
    {
        Logger.Dispose();
    }

    public ILogFormat Format => Logger.Format;

    public bool DebugLogEnabled
    {
        get => Logger.DebugLogEnabled;
        set => Logger.DebugLogEnabled = value;
    }

    public bool InfoLogEnabled
    {
        get => Logger.InfoLogEnabled;
        set => Logger.InfoLogEnabled = value;
    }

    public bool WarnLogEnabled
    {
        get => Logger.WarnLogEnabled;
        set => Logger.WarnLogEnabled = value;
    }

    public bool ErrorLogenabled
    {
        get => Logger.ErrorLogenabled;
        set => Logger.ErrorLogenabled = value;
    }

    /// Appends subContext to Context
    public void Log(string subContext, LogSeverity severity, object message, ILogFormat format)
    {
        Logger.Log($"{Context}/{subContext}", severity, message, format);
    }
}