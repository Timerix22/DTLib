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
using DTLib.Logging.New;


namespace DTLib.Tests;

public static class Program
{
    public static readonly Logging.ConsoleLogger OldLogger = new("logs", "DTLib.Tests");
    public static readonly ILogger NewLogger = new CompositeLogger(new ConsoleLogger(), new FileLogger(OldLogger.LogfileName));
    public static void Main()
    {
        Logging.PublicLog.LogEvent += OldLogger.Log;
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
        { NewLogger.LogError("Main", ex); }
        Console.ResetColor();
    }
}