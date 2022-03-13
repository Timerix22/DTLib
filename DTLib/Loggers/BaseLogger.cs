namespace DTLib.Loggers;

public abstract class BaseLogger
{
    public BaseLogger() { }
    public BaseLogger(string logfile) => (Logfile, WriteToFile) = (logfile,true);
    public BaseLogger(string dir, string programName)
        : this($"{dir}\\{programName}_{DateTime.Now}.log".Replace(':', '-').Replace(' ', '_')) { }


    public string Logfile { get; init; }
    public bool IsEnabled { get; private set; } = false;
    public bool WriteToFile { get; private set; } = false;
    protected readonly object statelocker = new();
    public void Disable() { lock (statelocker) IsEnabled = false; }
    public void Enable() { lock (statelocker) IsEnabled = true; }

    public abstract void Log(params string[] msg);
}
