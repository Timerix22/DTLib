global using System.Net.Sockets;
global using System;
global using System.Threading;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using DTLib.Extensions;
global using DTLib.Filesystem;
global using static DTLib.Logging.PublicLog;
using System.Diagnostics;
using System.Net.Http;

namespace DTLib.Network;

// 
// пара почти никогда не используемых методов
//
public static class OldNetwork
{


    // получает с сайта публичный ip
    public static string GetPublicIP() => new HttpClient().GetStringAsync("https://ifconfig.me/ip").GetAwaiter().GetResult();

    // пингует айпи с помощью встроенной в винду проги, возвращает задержку
    public static string PingIP(string address)
    {
        var proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = "/c @echo off & chcp 65001 >nul & ping -n 5 " + address;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();
        System.IO.StreamReader outStream = proc.StandardOutput;
        string rezult = outStream.ReadToEnd();
        rezult = rezult.Remove(0, rezult.LastIndexOf('=') + 2);
        return rezult.Remove(rezult.Length - 4);
    }

}
