namespace DTLib.Logging.New;

/// wrapper around ILogger and LoggerExtensions that stores context
public class LoggerContext
{
    public ILogger Logger;
    public readonly string Context;

    public LoggerContext(ILogger logger, string context)
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(object message)
        => Logger.LogError(Context, message);
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogException(Exception ex)
        => Logger.LogException(Context, ex);
}