namespace DTLib.Logging.New;

// вывод лога в консоль и файл
public class ConsoleLogger : ILogger
{
    readonly object consolelocker = new();
    public ILogFormat Format { get; }

    public ConsoleLogger(ILogFormat format) 
        => Format = format;
    
    public ConsoleLogger() : this(new DefaultLogFormat()) 
    {}

    
    public void Log(string context, LogSeverity severity, object message, ILogFormat format)
    {
        var msg = format.CreateMessage(context, severity, message);
        lock (consolelocker) 
            ColoredConsole.Write(ColorFromSeverity(severity),msg);
    }
    
    public void Log(string context, LogSeverity severity, object message)
        => Log(context, severity, message, Format);

    private static ConsoleColor ColorFromSeverity(LogSeverity severity)
        => severity switch
        {
            LogSeverity.Debug => ConsoleColor.Gray,
            LogSeverity.Info => ConsoleColor.White,
            LogSeverity.Warn => ConsoleColor.Yellow,
            LogSeverity.Error => ConsoleColor.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    
    public void Dispose()
    {
        lock (consolelocker) {}
    }

    ~ConsoleLogger() => Dispose();
}
