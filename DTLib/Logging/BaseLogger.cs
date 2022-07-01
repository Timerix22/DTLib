using System.IO;
using File = DTLib.Filesystem.File;

namespace DTLib.Logging;

public abstract class BaseLogger : IDisposable
{
    public BaseLogger() { }
    public BaseLogger(string logfile)
    {
        WriteToFile=true;
        LogfileName = logfile;
        LogfileStream = File.OpenWrite(logfile);
    }

    public BaseLogger(string dir, string programName)
        : this($"{dir}\\{programName}_{DateTime.Now}.log".Replace(':', '-').Replace(' ', '_')) { }
    
    public string LogfileName { get; protected set; }
    public FileStream LogfileStream { get; protected set; }

    protected readonly object _statelocker = new();
    
    private bool _isEnabled=true;
    public bool IsEnabled
    {
        get { lock (_statelocker) return _isEnabled; }
        set { lock (_statelocker) _isEnabled = value; }
    }

    private bool _writeToFile;
    public bool WriteToFile 
    {
        get { lock (_statelocker) return _writeToFile; }
        set { lock (_statelocker) _writeToFile = value; }
    }

    public abstract void Log(params string[] msg);

    public virtual void Dispose()
    {
        LogfileStream?.Flush();
        LogfileStream?.Close();
    }

    ~BaseLogger() => Dispose();
}
