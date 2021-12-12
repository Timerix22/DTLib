using DTLib.Filesystem;
using System;
using System.Text;

namespace DTLib
{
    // вывод лога в консоль и файл
    public class DefaultLogger
    {
        public DefaultLogger(string logfile) => Logfile = logfile;
        public DefaultLogger(string dir, string programName) => Logfile = $"{dir}\\{programName}_{DateTime.Now}.log".Replace(':', '-').Replace(' ', '_');

        public string Logfile { get; set; }

        private bool isEnabled=false;
        public void Enable() { lock (Logfile) isEnabled = true; }
        public void Disable() { lock (Logfile) isEnabled = false; }

        public void Log(params string[] msg)
        {
            lock (Logfile) if (!isEnabled) return;
            if (msg.Length == 1) msg[0] = "[" + DateTime.Now.ToString() + "]: " + msg[0];
            else msg[1] = "[" + DateTime.Now.ToString() + "]: " + msg[1];
            LogNoTime(msg);
        }

        public void LogNoTime(params string[] msg)
        {
            lock (Logfile) if (!isEnabled) return;
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
}
