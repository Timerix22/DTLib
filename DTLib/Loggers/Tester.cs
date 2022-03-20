using System.Diagnostics;
using System.Globalization;

namespace DTLib.Loggers;

public static class LogFunctions
{
    public static void LogOperationTime(string op_name, int repeats, Action operation)
    {
        Stopwatch clock = new();
        clock.Start();
        for (int i = 0; i < repeats; i++)
            operation();
        clock.Stop();
        double time=(double)(clock.ElapsedTicks)/Stopwatch.Frequency/repeats;
        LogNoTime("y",$"operation ","b",op_name,"y"," lasted ","b",time.ToString(CultureInfo.InvariantCulture),"y"," seconds");
    }
}