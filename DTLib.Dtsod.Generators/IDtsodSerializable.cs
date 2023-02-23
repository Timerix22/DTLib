global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using DTLib.Extensions;

namespace DTLib.Dtsod.Generic;

public interface IDtsodSerializable<out TSelf>
{
    public DtsodV23 ToDtsod();
    public TSelf FromDtsod();
}