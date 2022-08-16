using System.Diagnostics;
using System.Globalization;

namespace DTLib.Logging;

public static class Tester
{
    public static void LogOperationTime(string op_name, int repeats, Action operation)
    {
        Stopwatch clock = new();
        clock.Start();
        for (int i = 0; i < repeats; i++)
            operation();
        clock.Stop();
        double time=(double)(clock.ElapsedTicks)/Stopwatch.Frequency/repeats;
        Log("y",$"operation ","b",op_name,"y"," lasted ","b",time.ToString(MyTimeFormat.Instance),"y"," seconds");
    }
}