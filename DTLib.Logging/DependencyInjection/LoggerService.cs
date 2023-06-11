using Microsoft.Extensions.DependencyInjection;

namespace DTLib.Logging.DependencyInjection;

public class LoggerService<TCaller> : ServiceDescriptor
{
    // ReSharper disable once RedundantNameQualifier
    public LoggerService(DTLib.Logging.ILogger logger) : base( 
        typeof(Microsoft.Extensions.Logging.ILogger<TCaller>), 
        new MyLoggerWrapper<TCaller>(logger))
    {
    }
}