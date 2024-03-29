﻿namespace DTLib.Logging;

public class FileLogger : ILogger
{
    public bool DebugLogEnabled { get; set; } = false;
    public bool InfoLogEnabled { get; set; } = true;
    public bool WarnLogEnabled { get; set; } = true;
    public bool ErrorLogEnabled { get; set; } = true;
    public ILogFormat Format { get; set; }
    
    public IOPath LogfileName { get; protected set; }
    public System.IO.FileStream LogfileStream { get; protected set; }
    
    public FileLogger(IOPath logfile, ILogFormat format)
    {
        Format = format;
        LogfileName = logfile;
        LogfileStream = File.OpenAppend(logfile);
    }

    public FileLogger(IOPath logfile) : this(logfile, new DefaultLogFormat())
    {}

    public FileLogger(IOPath dir, IOPath programName, ILogFormat format)
        : this($"{Path.Concat(dir, programName)}_{DateTime.Now.ToString(MyTimeFormat.ForFileNames)}.log", format)
    {}

    public FileLogger(IOPath dir, IOPath programName) : this(dir, programName, new DefaultLogFormat())
    {}

    public void Log(string context, LogSeverity severity, object message, ILogFormat format)
    {
        if(!this.CheckSeverity(severity))
            return;
        
        var msg = format.CreateMessage(context, severity, message);
        lock (LogfileStream)
        {
            LogfileStream.FluentWriteString(msg)
                .FluentWriteByte('\n'.ToByte())
                .Flush();
        }
    }

    public virtual void Dispose()
    {
        try
        {
            LogfileStream?.Flush();
            LogfileStream?.Dispose();
        }
        catch (ObjectDisposedException) { }
    }

    ~FileLogger() => Dispose();
}
