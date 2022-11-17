namespace DTLib.Logging.New;

public class FileLogger : ILogger
{
    public bool DebugLogEnabled { get; set; } = true;
    public bool InfoLogEnabled { get; set; } = true;
    public bool WarnLogEnabled { get; set; } = true;
    public bool ErrorLogenabled { get; set; } = true;
    public ILogFormat Format { get; }
    
    public string LogfileName { get; protected set; }
    public System.IO.FileStream LogfileStream { get; protected set; }
    
    public FileLogger(string logfile, ILogFormat format)
    {
        Format = format;
        LogfileName = logfile;
        LogfileStream = File.OpenAppend(logfile);
    }

    public FileLogger(string logfile) : this(logfile, new DefaultLogFormat())
    {}

    public FileLogger(string dir, string programName, ILogFormat format)
        : this($"{dir}{Путь.Разд}{programName}_{DateTime.Now.ToString(MyTimeFormat.ForFileNames)}.log", format)
    {}

    public FileLogger(string dir, string programName) : this(dir, programName, new DefaultLogFormat())
    {}

    public void Log(string context, LogSeverity severity, object message, ILogFormat format)
    {
        if(!this.CheckSeverity(severity))
            return;
        
        var msg = format.CreateMessage(context, severity, format).ToBytes(StringConverter.UTF8);
        lock (LogfileStream)
        {
            LogfileStream.Write(msg);
            LogfileStream.Flush();
        }
    }
    
    public void Log(string context, LogSeverity severity, object message)
        => Log(context, severity, message, Format);

    public virtual void Dispose()
    {
        try 
        {
            LogfileStream?.Flush();
            LogfileStream?.Close();
            LogfileStream?.Dispose();
        }
        catch (ObjectDisposedException) { }
    }

    ~FileLogger() => Dispose();
}
