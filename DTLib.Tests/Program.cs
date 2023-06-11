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
global using DTLib.Console;
global using DTLib.Logging;

namespace DTLib.Tests;

public static class Program
{
    public static ILogger Logger = new CompositeLogger(new ConsoleLogger(),
            new FileLogger("logs", "DTLib.Tests"))

    {
        DebugLogEnabled = true
    };

    public static void Main(string[] args)
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        System.Console.InputEncoding = Encoding.UTF8;
        var mainContext = new ContextLogger("Main", Logger);
        DTLibInternalLogging.SetLogger(Logger);
        
        try
        {
            new LaunchArgumentParser().WithNoExit().ParseAndHandle(args);
            TestDtsodV23.TestAll();
            // TestPInvoke.TestAll();
            // TestAutoarr.TestAll();
            // TestDtsodV24.TestAll();
        }
        catch(LaunchArgumentParser.ExitAfterHelpException)
        { }
        catch (Exception ex)
        { mainContext.LogError(ex); }
        
        System.Console.ResetColor();
    }
}