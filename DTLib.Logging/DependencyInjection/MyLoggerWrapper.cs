using DTLib.Logging;
using Microsoft.Extensions.Logging;
// ReSharper disable RedundantNameQualifier

namespace DTLib.Logging.DependencyInjection;

public class MyLoggerWrapper<TCaller> : Microsoft.Extensions.Logging.ILogger<TCaller>
{
    public DTLib.Logging.ILogger Logger;
    public MyLoggerWrapper(DTLib.Logging.ILogger logger)=>
        Logger = logger;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        string message = formatter(state, exception);
        Logger.Log(typeof(TCaller).Name, LogSeverity_FromLogLevel(logLevel), message);
    }

    private bool _isEnabled=true;
    public bool IsEnabled(LogLevel logLevel) => _isEnabled;

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    static LogSeverity LogSeverity_FromLogLevel(LogLevel l)
        => l switch
        {
            LogLevel.Trace => LogSeverity.Debug,
            LogLevel.Debug => LogSeverity.Debug,
            LogLevel.Information => LogSeverity.Info,
            LogLevel.Warning => LogSeverity.Warn,
            LogLevel.Error => LogSeverity.Error,
            LogLevel.Critical => LogSeverity.Error,
            LogLevel.None => throw new NotImplementedException("LogLevel.None is not supported"),
            _ => throw new ArgumentOutOfRangeException(nameof(l), l, null)
        }
    ;
}