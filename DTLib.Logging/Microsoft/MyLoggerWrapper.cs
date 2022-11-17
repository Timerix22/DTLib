using Microsoft.Extensions.Logging;

namespace DTLib.Logging.New.Microsoft;

internal class MyLoggerWrapper<TCaller> : ILogger<TCaller>
{
    private ILogger _logger;
    public MyLoggerWrapper(ILogger logger)=>
        _logger = logger;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        string message = formatter(state, exception);
        _logger.Log(nameof(TCaller), LogSeverity_FromLogLevel(logLevel), message);
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