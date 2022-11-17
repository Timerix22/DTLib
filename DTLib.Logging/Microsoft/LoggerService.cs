using Microsoft.Extensions.DependencyInjection;

namespace DTLib.Logging.New.Microsoft;

public class LoggerService<TCaller> : ServiceDescriptor
{
    ILogger _logger;

    public LoggerService(DTLib.Logging.New.ILogger logger) : base( typeof(ILogger), new MyLoggerWrapper<TCaller>(logger))
    {
        _logger = logger;
    }
}