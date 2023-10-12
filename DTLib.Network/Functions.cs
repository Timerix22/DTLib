global using System.Net.Sockets;
global using System;
global using System.Threading;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using DTLib.Extensions;
global using DTLib.Filesystem;
global using static DTLib.Logging.InternalLog;
using System.Diagnostics;
using System.Net.Http;

namespace DTLib.Network;

public static class Functions
{
    /// gets public ip of this machine from ifconfig.me
    public static string GetPublicIP() => new HttpClient().GetStringAsync("https://ifconfig.me/ip").GetAwaiter().GetResult();
}
