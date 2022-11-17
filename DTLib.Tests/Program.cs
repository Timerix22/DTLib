global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using DTLib;
global using DTLib.Extensions;
global using DTLib.Filesystem;
global using DTLib.Dtsod;
global using static DTLib.Tests.TesterLog;
global using static DTLib.Tests.Program;
using DTLib.Logging;


namespace DTLib.Tests;

public static class Program
{
    public static readonly ConsoleLogger Info = new("logs", "DTLib.Tests");
    public static void Main()
    {
        PublicLog.LogEvent += Info.Log;
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title="tester";
        try
        {
            TestPInvoke.TestAll();
            TestAutoarr.TestAll();
            TestDtsodV23.TestAll();
            TestDtsodV24.TestAll();
        }
        catch (Exception ex)
        { Info.Log("r", ex.ToString()); }
        Console.ResetColor();
    }
}