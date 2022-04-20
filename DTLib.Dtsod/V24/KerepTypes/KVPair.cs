using System.Runtime.InteropServices;

namespace DTLib.Dtsod.V24.KerepTypes;

[StructLayout(LayoutKind.Sequential)]
public struct KVPair
{
    public IntPtr key;
    public Unitype value;

    public KVPair(IntPtr k, Unitype v)
    {
        key = k;
        value = v;
    }
    public KVPair(string k, Unitype v)
    {
        key = k.ToHGlobalUTF8();
        value = v;
    }
    
    public override string ToString()
    {
        return $"{{{Marshal.PtrToStringUTF8(key)}, {value}}}";
    }
}