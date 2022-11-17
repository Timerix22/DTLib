﻿namespace DTLib.Logging.New;

// вывод лога в консоль и файл
public class ConsoleLogger : ILogger
{
    public bool DebugLogEnabled { get; set; } = false;
    public bool InfoLogEnabled { get; set; } = true;
    public bool WarnLogEnabled { get; set; } = true;
    public bool ErrorLogenabled { get; set; } = true;
    public ILogFormat Format { get; }
    
    readonly object consolelocker = new();

    public ConsoleLogger(ILogFormat format) 
        => Format = format;
    
    public ConsoleLogger() : this(new DefaultLogFormat()) 
    {}

    
    public void Log(string context, LogSeverity severity, object message, ILogFormat format)
    {
        if(!this.CheckSeverity(severity))
            return;
        
        var msg = format.CreateMessage(context, severity, message);
        lock (consolelocker) 
            ColoredConsole.Write(ColorFromSeverity(severity),msg);
    }

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