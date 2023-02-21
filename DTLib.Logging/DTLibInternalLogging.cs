global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Runtime.CompilerServices;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using DTLib.Extensions;
global using DTLib.Filesystem;

namespace DTLib.Logging.New;

/// this class can be used to setup logger for DTLib debug log messages
public static class DTLibInternalLogging
{
    private static ContextLogger _loggerContext;

    public static void SetLogger(ILogger logger)
    {
        _loggerContext = new ContextLogger("DTLib",logger);
        PublicLog.LogEvent+=LogHandler;
    }

    private static void LogHandler(string[] msg)
    {
        if (msg.Length == 1)
        {
            _loggerContext.LogDebug(msg[0]);
            return;
        }

        StringBuilder b = new();
        for (int i = 1; i < msg.Length; i++)
            b.Append(msg[i]);
        _loggerContext.LogDebug(b.ToString());
    }
}