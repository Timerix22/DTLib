global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using DTLib;
global using DTLib.Extensions;
global using DTLib.Experimental;
global using DTLib.Filesystem;
global using DTLib.Dtsod;
using DTLib.Loggers;
using TestProgram.DtsodV2X;

namespace TestProgram;

static class Program
{
    public static DefaultLogger Info = new();
    static public void Main()
    {

        Info.Enable();
        PublicLog.LogEvent += Info.Log;
        PublicLog.LogNoTimeEvent += Info.LogNoTime;
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title="tester";
        try
        {
            Info.Log("g","-------[DTLib tester]-------");
            //TestDtsodV23.TestAll();
            DictTest.Test();
        }
        catch (Exception ex)
        { Info.Log("r", ex.ToString()); }
        Console.ResetColor();
    }
}