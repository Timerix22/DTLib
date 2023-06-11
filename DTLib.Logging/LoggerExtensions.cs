using DTLib.Ben.Demystifier;

namespace DTLib.Logging;

public static class LoggerExtensions
{
    // replaces same overload in every ILogger instaance
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Log(this ILogger logger, string context, LogSeverity severity, object message)
        => logger.Log(context, severity, message, logger.Format);
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogDebug(this ILogger logger, string context, object message)
    => logger.Log(context, LogSeverity.Debug, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogInfo(this ILogger logger, string context, object message)
        => logger.Log(context, LogSeverity.Info, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogWarn(this ILogger logger, string context, object message)
        => logger.Log(context, LogSeverity.Warn, message);
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogWarn(this ILogger logger, string context, Exception ex)
        => logger.Log(context, LogSeverity.Warn, ex.ToStringDemystified());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogError(this ILogger logger, string context, object message)
        => logger.Log(context, LogSeverity.Error, message);
    
    
    /// uses Ben.Demystifier to serialize exception
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogError(this ILogger logger, string context, Exception ex)
        => logger.Log(context, LogSeverity.Error, ex.ToStringDemystified());
}