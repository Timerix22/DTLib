using System.Diagnostics;
using DTLib.Logging;

namespace DTLib.Tests;

public static class TesterLog
{
    public static void LogOperationTime(string op_name, int repeats, Action operation)
    {
        Stopwatch clock = new();
        clock.Start();
        for (int i = 0; i < repeats; i++)
            operation();
        clock.Stop();
        double time=(double)(clock.ElapsedTicks)/Stopwatch.Frequency/repeats;
        InternalLog.Log("y",$"operation ","b",op_name,"y"," lasted ","b",time.ToString(MyTimeFormat.ForText),"y"," seconds");
    }
}