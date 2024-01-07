using Microsoft.Extensions.DependencyInjection;

namespace DTLib.Logging.Microsoft;

public class LoggerService<TCaller> : ServiceDescriptor
{
    // ReSharper disable once RedundantNameQualifier
    public LoggerService(DTLib.Logging.ILogger logger) : base( 
        typeof(global::Microsoft.Extensions.Logging.ILogger<TCaller>), 
        new MyLoggerWrapper<TCaller>(logger))
    {
    }
}