using System;
using System.Text;
using DTLib.Filesystem;

namespace DTLib
{
    // вывод лога в консоль и файл
    public static class DefaultLogger
    {
        public static void SetLogfile(string dir, string programName)
            => Logfile = $"{dir}\\{programName}_{DateTime.Now}.log".Replace(':', '-').Replace(' ', '_');

        static public string Logfile { get; set; }
        public static void Log(params string[] msg)
        {
            if (msg.Length == 1) msg[0] = "[" + DateTime.Now.ToString() + "]: " + msg[0];
            else msg[1] = "[" + DateTime.Now.ToString() + "]: " + msg[1];
            LogNoTime(msg);
        }
        public static void LogNoTime(params string[] msg)
        {
            lock (Logfile)
            {
                ColoredConsole.Write(msg);
                if (msg.Length == 1) File.AppendAllText(Logfile, msg[0]);
                else
                {
                    StringBuilder strB = new();
                    for (ushort i = 0; i < msg.Length; i++)
                        strB.Append(msg[++i]);
                    File.AppendAllText(Logfile, strB.ToString());
                }
            }
        }
    }
}
