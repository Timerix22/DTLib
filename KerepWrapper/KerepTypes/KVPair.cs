using System.Runtime.InteropServices;
using DTLib.Extensions;

namespace KerepWrapper.KerepTypes;

[StructLayout(LayoutKind.Sequential)]
public struct KVPair
{
    public DtsodPtr key;
    public Unitype value;

    public KVPair(DtsodPtr k, Unitype v)
    {
        key = k;
        value = v;
    }
    public KVPair(string k, Unitype v)
    {
        key = k.StringToHGlobalUTF8();
        value = v;
    }
    
    public override string ToString()
    {
        return $"{{{Unmanaged.HGlobalUTF8ToString(key)}, {value}}}";
    }
}