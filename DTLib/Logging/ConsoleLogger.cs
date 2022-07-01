using System.Globalization;

namespace DTLib.Logging;

// вывод лога в консоль и файл
public class ConsoleLogger : BaseLogger
{
    public ConsoleLogger() : base() {}
    public ConsoleLogger(string logfile) : base(logfile){}
    public ConsoleLogger(string dir, string programName) : base(dir, programName) {}
    
    
    readonly object consolelocker = new();
    
    public override void Log(params string[] msg)
    {
        if (!IsEnabled) return;
        var strb = new StringBuilder();
        strb.Append('[').Append(DateTime.Now.ToString(CultureInfo.InvariantCulture)).Append("]: ");
        int index = msg.Length == 1 ? 0 : 1;
        strb.Append(msg[index]);
        msg[index] = strb.ToString();
        // write to console
        lock (consolelocker)
        {
            ColoredConsole.Write(msg);
            Console.WriteLine();
        }
        // write to file
        if (!WriteToFile) return;
        if (msg.Length == 1)
            lock (LogfileStream)
            {
                LogfileStream.Write(msg[0].ToBytes());
                LogfileStream.WriteByte('\n'.ToByte());
            }
        else
        {
            for (ushort i = 3; i < msg.Length; i+=2)
                strb.Append(msg[i]);
            strb.Append('\n');
            lock (LogfileStream)
                LogfileStream.Write(strb.ToString().ToBytes());
        }
    }
}
