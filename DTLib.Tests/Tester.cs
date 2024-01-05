using System.Diagnostics;

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
        Logger.LogInfo(nameof(TesterLog), $"operation {op_name} lasted {time.ToString(MyTimeFormat.ForText)} seconds");
    }
}