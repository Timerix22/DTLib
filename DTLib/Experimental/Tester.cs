﻿using System.Diagnostics;

namespace DTLib.Experimental;

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
        LogNoTime("y",$"operation ","b",op_name,"y"," took ","b",time.ToString(),"y"," seconds");
    }
}