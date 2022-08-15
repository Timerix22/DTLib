namespace DTLib.Logging;

// вывод лога в консоль и файл
public class ConsoleLogger : FileLogger
{
    public ConsoleLogger(string logfile) : base(logfile){}
    public ConsoleLogger(string dir, string programName) : base(dir, programName) {}
    
    
    readonly object consolelocker = new();
    
    public override void Log(params string[] msg)
    {
        // write to file
        base.Log(msg);
        // append timestamp
        var strb = new StringBuilder();
        strb.Append('[').Append(LastLogMessageTime).Append("]: ");
        int index = msg.Length == 1 ? 0 : 1;
        strb.Append(msg[index]);
        msg[index] = strb.ToString();
        // write to console
        lock (consolelocker)
        {
            ColoredConsole.Write(msg);
            Console.WriteLine();
        }
    }
}
