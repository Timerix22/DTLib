using Microsoft.Extensions.DependencyInjection;

namespace DTLib.Logging.DependencyInjection;

public class LoggerService<TCaller> : ServiceDescriptor
{
    DTLib.Logging.New.ILogger _logger;

    public LoggerService(DTLib.Logging.New.ILogger logger) : base( 
        typeof(Microsoft.Extensions.Logging.ILogger<TCaller>), 
        new MyLoggerWrapper<TCaller>(logger))
    {
        _logger = logger;
    }
}