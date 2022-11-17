namespace DTLib.Logging.New;

public class FileLogger : ILogger
{
    public bool DebugLogEnabled { get; set; } = false;
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
        
        var msg = format.CreateMessage(context, severity, message);
        lock (LogfileStream)
        {
            LogfileStream.Write(msg.ToBytes(StringConverter.UTF8));
            LogfileStream.Flush();
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
