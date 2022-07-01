using System.IO;
using File = DTLib.Filesystem.File;

namespace DTLib.Loggers;

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


    public string LogfileName;
    public FileStream LogfileStream { get; init; }
    public bool IsEnabled { get; private set; } = false;
    public bool WriteToFile { get; private set; } = false;
    protected readonly object statelocker = new();
    public void Disable() { lock (statelocker) IsEnabled = false; }
    public void Enable() { lock (statelocker) IsEnabled = true; }

    public abstract void Log(params string[] msg);

    public virtual void Dispose()
    {
        LogfileStream?.Flush();
        LogfileStream?.Close();
    }

    ~BaseLogger() => Dispose();
}
