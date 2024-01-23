namespace DTLib.Logging;

public static class LogSeverityHelper
{
    public static bool CheckSeverity(this ILogger logger, LogSeverity severity)
        => severity switch
        {
            LogSeverity.Debug => logger.DebugLogEnabled,
            LogSeverity.Info => logger.InfoLogEnabled,
            LogSeverity.Warn => logger.WarnLogEnabled,
            LogSeverity.Error => logger.ErrorLogEnabled,
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, "unknown severity")
        };
}