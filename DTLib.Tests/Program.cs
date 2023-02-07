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
using DTLib.Console;
using DTLib.Logging.New;

namespace DTLib.Tests;

public static class Program
{
    public static Logging.ConsoleLogger OldLogger = new("logs", "DTLib.Tests");
    public static ILogger Logger;
    public static void Main(string[] args)
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        System.Console.InputEncoding = Encoding.UTF8;
        Logger=new CompositeLogger(new ConsoleLogger(), 
            new FileLogger("logs", "DTLib.Tests"));
        var mainContext = new ContextLogger(Logger, "Main");
        DTLibInternalLogging.SetLogger(Logger);
        
        try
        {
            IOPath path = "file";
            IOPath path2 = "dir/"+ path + "_temp";
            File.Create(path);
            File.Copy(path, path2, true);
            System.Console.WriteLine("{0} was copied to {1}.", path, path2);
            Directory.Copy(path2.ParentDir(), "dir2/c/", true);
            System.Console.WriteLine($"dir/c/:\n"+Directory.GetAllFiles("dir2\\c").MergeToString("\n\t"));
            return;
            
            new LaunchArgumentParser().WithNoExit().ParseAndHandle(args);
            TestPInvoke.TestAll();
            TestAutoarr.TestAll();
            TestDtsodV23.TestAll();
            TestDtsodV24.TestAll();
        }
        catch(LaunchArgumentParser.ExitAfterHelpException)
        { }
        catch (Exception ex)
        { mainContext.LogError(ex); }
        
        System.Console.ResetColor();
    }
}