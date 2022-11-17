using Ben.Demystifier;

namespace DTLib.Logging.New;

public static class LoggerExtensions
{
    public static void Log(this ILogger logger, string context, LogSeverity severity, object message)
        => logger.Log(context, severity, message, logger.Format);

    public static void LogException(this ILogger logger, string context, Exception ex)
        => logger.Log(context, LogSeverity.Error, ex.Demystify());
}