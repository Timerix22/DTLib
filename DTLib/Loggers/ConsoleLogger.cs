using System.Globalization;

namespace DTLib.Loggers;

// вывод лога в консоль и файл
public class ConsoleLogger : BaseLogger
{
    public ConsoleLogger() : base() {}
    public ConsoleLogger(string logfile) : base(logfile){}
    public ConsoleLogger(string dir, string programName) : base(dir, programName) {}

    
    public override void Log(params string[] msg)
    {
        lock (statelocker) if (!IsEnabled) return;
        if (msg.Length == 1) msg[0] = "[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "]: " + msg[0];
        else msg[1] = "[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "]: " + msg[1];
        LogNoTime(msg);
    }

    
    readonly object consolelocker = new();
    
    public void LogNoTime(params string[] msg)
    {
        lock (statelocker) if (!IsEnabled) return;
        msg[msg.Length - 1] += '\n';
        lock (consolelocker)
            ColoredConsole.Write(msg);
        if (WriteToFile)
        {
            if (msg.Length == 1)
                lock (LogfileStream) LogfileStream.Write(msg[0].ToBytes());
            else
            {
                StringBuilder strB = new();
                for (ushort i = 0; i < msg.Length; i++)
                    strB.Append(msg[++i]);
                lock (LogfileStream) LogfileStream.Write(strB.ToString().ToBytes());
            }
        }
    }
    
    public void LogAsync(params string[] msg) => Task.Run(() => Log(msg));
}
