using System.Globalization;

namespace DTLib.Logging;

public class FileLogger : IDisposable
{
    public FileLogger(string logfile)
    {
        LogfileName = logfile;
        LogfileStream = File.OpenAppend(logfile);
    }

    public FileLogger(string dir, string programName)
        : this($"{dir}{Путь.Разд}{programName}_{DateTime.Now}.log".Replace(':', '-').Replace(' ', '_')) { }
    
    public string LogfileName { get; protected set; }
    public System.IO.FileStream LogfileStream { get; protected set; }
    protected string LastLogMessageTime;
    
    
    public virtual void Log(params string[] msg)
    {
        lock (LogfileStream)
        {
            LastLogMessageTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            LogfileStream.WriteByte('['.ToByte());
            LogfileStream.Write(LastLogMessageTime.ToBytes());
            LogfileStream.Write("]: ".ToBytes());
            if (msg.Length == 1)
                LogfileStream.Write(msg[0].ToBytes());
            else
            {
                var strb = new StringBuilder();
                for (ushort i = 1; i < msg.Length; i += 2)
                    strb.Append(msg[i]);
                LogfileStream.Write(strb.ToString().ToBytes());
            }
            LogfileStream.WriteByte('\n'.ToByte());
        }
    }

    public virtual void Dispose()
    {
        LogfileStream?.Flush();
        LogfileStream?.Close();
    }

    ~FileLogger() => Dispose();
}
