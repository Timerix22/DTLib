namespace DTLib.Loggers;

public abstract class BaseLogger
{
    public string Logfile { get; init; }
    public BaseLogger(string logfile) => Logfile = logfile;
    public BaseLogger(string dir, string programName) => Logfile = $"{dir}\\{programName}_{DateTime.Now}.log".Replace(':', '-').Replace(' ', '_');


    public bool IsEnabled { get; private set; } = false;
    readonly protected object statelocker = new();
    public void Disable() { lock (statelocker) IsEnabled = false; }
    public void Enable() { lock (statelocker) IsEnabled = true; }

    public abstract void Log(params string[] msg);
}
