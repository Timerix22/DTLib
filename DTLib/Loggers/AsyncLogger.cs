using System.Globalization;

namespace DTLib.Loggers;

// вывод лога в консоль и файл
public class AsyncLogger : BaseLogger
{
    public AsyncLogger(string logfile) : base(logfile) { }

    public AsyncLogger(string dir, string programName) : base(dir, programName) { }

    readonly object consolelocker = new();
    public override void Log(params string[] msg)
    {
        lock (statelocker) if (!IsEnabled) return;
        // добавление даты
        if (msg.Length == 1) msg[0] = "[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "]: " + msg[0];
        else msg[1] = "[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "]: " + msg[1];
        // перенос строки
        msg[msg.Length - 1] += '\n';
        // вывод в консоль
        lock (consolelocker)
            ColoredConsole.Write(msg);
        // вывод в файл
        if (msg.Length == 1)
            lock (Logfile) File.AppendAllText(Logfile, msg[0]);
        else
        {
            StringBuilder strB = new();
            for (ushort i = 0; i < msg.Length; i++)
                strB.Append(msg[++i]);
            lock (Logfile) File.AppendAllText(Logfile, strB.ToString());
        }
    }
    public void LogAsync(params string[] msg) => Task.Run(() => Log(msg));
}
