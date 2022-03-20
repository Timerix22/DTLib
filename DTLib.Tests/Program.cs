global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using DTLib;
global using DTLib.Extensions;
global using static DTLib.Loggers.LogFunctions;
global using DTLib.Filesystem;
global using DTLib.Dtsod;
global using static DTLib.Tests.Program;
using DTLib.Loggers;


namespace DTLib.Tests;

public static class Program
{
    public static readonly DefaultLogger Info = new();
    public static void Main()
    {

        Info.Enable();
        PublicLog.LogEvent += Info.Log;
        PublicLog.LogNoTimeEvent += Info.LogNoTime;
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title="tester";
        try
        {
            Info.Log("c", "-------------[DTLib.Tests]-------------");
            //TestDtsodV23.TestAll();
            DictTest.Test();
        }
        catch (Exception ex)
        { Info.Log("r", ex.ToString()); }
        Console.ResetColor();
    }
}