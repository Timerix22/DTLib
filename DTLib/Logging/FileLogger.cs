namespace DTLib.Logging;

public class FileLogger : IDisposable
{
    public FileLogger(string logfile)
    {
        LogfileName = logfile;
        LogfileStream = File.OpenAppend(logfile);
    }

    public FileLogger(string dir, string programName)
        : this($"{dir}{Path.Sep}{programName}_{DateTime.Now.ToString(MyTimeFormat.ForFileNames)}.log") { }
    
    public string LogfileName { get; protected set; }
    public System.IO.FileStream LogfileStream { get; protected set; }
    protected string LastLogMessageTime;
    
    
    public virtual void Log(params string[] msg)
    {
        lock (LogfileStream)
        {
            LastLogMessageTime = DateTime.Now.ToString(MyTimeFormat.ForText);
            LogfileStream.FluentWriteString("[")
                .FluentWriteString(LastLogMessageTime)
                .FluentWriteString("]: ");
            if (msg.Length == 1)
                LogfileStream.FluentWriteString(msg[0]);
            else
            {
                var strb = new StringBuilder();
                for (ushort i = 1; i < msg.Length; i += 2)
                    strb.Append(msg[i]);
                LogfileStream.FluentWriteString(strb.ToString());
            }
            LogfileStream.FluentWriteString("\n").Flush();
        }
    }

    public virtual void Dispose()
    {
        try 
        {
            LogfileStream?.Flush();
            LogfileStream?.Close();
        }
        catch (ObjectDisposedException) { }
    }

    ~FileLogger() => Dispose();
}
