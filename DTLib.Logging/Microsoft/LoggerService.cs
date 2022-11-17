using Microsoft.Extensions.Logging;

namespace DTLib.Logging.New.Microsoft;

public class LoggerService<TCaller> : IServiceProvider
{
    ILogger _logger;

    public LoggerService(ILogger logger)
    {
        _logger = logger;
    }

    public object GetService(Type serviceType)
    {
        return new MyLoggerWrapper<TCaller>(_logger);
    }
}