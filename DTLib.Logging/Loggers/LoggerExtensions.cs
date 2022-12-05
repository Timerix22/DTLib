using Ben.Demystifier;

namespace DTLib.Logging.New;

public static class LoggerExtensions
{
    // replaces same overload in every ILogger instaance
    public static void Log(this ILogger logger, string context, LogSeverity severity, object message)
        => logger.Log(context, severity, message, logger.Format);

    public static void LogDebug(this ILogger logger, string context, object message)
    => logger.Log(context, LogSeverity.Debug, message);
    public static void LogInfo(this ILogger logger, string context, object message)
        => logger.Log(context, LogSeverity.Info, message);
    public static void LogWarn(this ILogger logger, string context, object message)
        => logger.Log(context, LogSeverity.Warn, message);
    
    public static void LogError(this ILogger logger, string context, object message)
        => logger.Log(context, LogSeverity.Error, message);
    
    /// uses Ben.Demystifier to serialize exception
    public static void LogException(this ILogger logger, string context, Exception ex)
        => logger.Log(context, LogSeverity.Error, ex.Demystify());
}