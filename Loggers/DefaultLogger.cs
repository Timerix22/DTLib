namespace DTLib.Loggers;

// вывод лога в консоль и файл
public class DefaultLogger : BaseLogger
{
    public DefaultLogger(string logfile) : base(logfile) { }

    public DefaultLogger(string dir, string programName) : base(dir, programName) { }

    public override void Log(params string[] msg)
    {
        lock (Logfile) if (!IsEnabled) return;
        if (msg.Length == 1) msg[0] = "[" + DateTime.Now.ToString() + "]: " + msg[0];
        else msg[1] = "[" + DateTime.Now.ToString() + "]: " + msg[1];
        LogNoTime(msg);
    }

    public void LogNoTime(params string[] msg)
    {
        lock (Logfile) if (!IsEnabled) return;
        ColoredConsole.Write(msg);
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
}
