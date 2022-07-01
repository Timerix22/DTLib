global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using DTLib.Extensions;
global using static DTLib.Logging.PublicLog;

namespace DTLib.Dtsod;

public interface IDtsod
{
    public DtsodVersion Version { get; }
    public IDictionary<string, dynamic> ToDictionary();
    public dynamic this[string s] { get; set; }
}
