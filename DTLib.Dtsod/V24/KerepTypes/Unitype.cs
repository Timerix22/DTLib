using System.Runtime.InteropServices;
using DTLib.Dtsod.V24.Autoarr;

namespace DTLib.Dtsod.V24.KerepTypes;

[StructLayout(LayoutKind.Explicit)]
public struct Unitype
{
    [FieldOffset(0)] public long Int64;
    [FieldOffset(0)] public ulong UInt64;
    [FieldOffset(0)] public double Float64;
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)] public bool Bool;
    [FieldOffset(0)] public IntPtr VoidPtr;
    [FieldOffset(8)] public KerepTypeCode TypeCode;

    public Unitype(object v) : this()
    {
        TypeCode = KerepTypeHelper.GetKerepTypeCode(v);
        switch (TypeCode)
        {
            case KerepTypeCode.Bool:
                Bool = (bool) v;
                break;
            case KerepTypeCode.UInt8:
            case KerepTypeCode.UInt16:
            case KerepTypeCode.UInt32:
                UInt64 = v.ToULong();
                TypeCode = KerepTypeCode.UInt64;
                break;
            case KerepTypeCode.UInt64:
                UInt64 = (ulong) v;
                break;
            case KerepTypeCode.Int8:
            case KerepTypeCode.Int16:
            case KerepTypeCode.Int32:
                Int64 = v.ToLong();
                TypeCode = KerepTypeCode.Int64;
                break;
            case KerepTypeCode.Int64:
                Int64 = (long) v;
                break;
            case KerepTypeCode.Float32:
                Float64 = v.ToDouble();
                TypeCode = KerepTypeCode.Float64;
                break;
            case KerepTypeCode.Float64:
                Float64 = (double) v;
                break;
            case KerepTypeCode.CharPtr:
                VoidPtr = ((string)v).ToHGlobalUTF8();
                break;
            case KerepTypeCode.AutoarrUnitypePtr:
                TypeCode = KerepTypeCode.AutoarrUnitypePtr;
                var ar = new Autoarr<Unitype>(64,1024,false);
                foreach (var sub in (List<object>)v)
                    ar.Add(new Unitype(sub));
                VoidPtr = ar.UnmanagedPtr;
                break;
            case KerepTypeCode.HashtablePtr:
                TypeCode = KerepTypeCode.HashtablePtr;
                var ht = new DtsodV24((IDictionary<string,object>)v,false);
                VoidPtr = ht.UnmanagedPtr;
                break;
            default: throw new Exception($"can't box value of type {TypeCode}");
        }
    }
    
    /*public Unitype(bool v) : this()
    {
        TypeCode = KerepTypeCode.Bool;
        Bool = v;
    }
    public Unitype(int v) : this()
    {
        TypeCode = KerepTypeCode.Int64;
        Int64 = v;
    }
    public Unitype(uint v) : this()
    {
        TypeCode = KerepTypeCode.UInt64;
        UInt64 = v;
    }
    public Unitype(double v) : this()
    {
        TypeCode = KerepTypeCode.Float64;
        Float64 = v;
    }
    public Unitype(string s) : this()
    {
        TypeCode = KerepTypeCode.CharPtr;
        VoidPtr = s.ToHGlobalUTF8();
    }
    
    public Unitype(Autoarr<Unitype> v) : this()
    {
        TypeCode = KerepTypeCode.AutoarrUnitypePtr;
        VoidPtr = v.UnmanagedPtr;
    }
    public Unitype(Autoarr<KVPair> v) : this()
    {
        TypeCode = KerepTypeCode.AutoarrKVPairPtr;
        VoidPtr = v.UnmanagedPtr;
    }
    public Unitype(DtsodV24 v) : this()
    {
        TypeCode = KerepTypeCode.HashtablePtr;
        VoidPtr = v.UnmanagedPtr;
    }*/

    public dynamic ToDynamic()
    {
        switch (TypeCode)
        {
            case KerepTypeCode.Null: return null;
            case KerepTypeCode.Bool: return Bool;
            case KerepTypeCode.Int64: return Int64;
            case KerepTypeCode.UInt64: return UInt64;
            case KerepTypeCode.Float64: return Float64;
            case KerepTypeCode.CharPtr: return Marshal.PtrToStringUTF8(VoidPtr);
            case KerepTypeCode.AutoarrUnitypePtr: return new Autoarr<Unitype>(VoidPtr, false);
            case KerepTypeCode.AutoarrKVPairPtr: return new Autoarr<KVPair>(VoidPtr, false);
            case KerepTypeCode.HashtablePtr: return new DtsodV24(VoidPtr);
            default: throw new Exception($"can't unbox value of type {TypeCode}");
        }
    }

    public override string ToString()
    {
        switch (TypeCode)
        {
            case KerepTypeCode.Null: return "{Null}";
            case KerepTypeCode.Bool: return $"{{Bool:{Bool}}}";
            case KerepTypeCode.Int64: return $"{{Int64:{Int64}}}";
            case KerepTypeCode.UInt64: return $"{{UInt64:{UInt64}}}";
            case KerepTypeCode.Float64: return $"{{Float64:{Float64}}}";
            case KerepTypeCode.CharPtr: return $"{{CharPtr:{Marshal.PtrToStringUTF8(VoidPtr)}}}";
            case KerepTypeCode.AutoarrUnitypePtr: return $"{{AutoarrUnitypePtr:{VoidPtr.ToString()}}}";
            case KerepTypeCode.AutoarrKVPairPtr: return $"{{AutoarrKVPairPtr:{VoidPtr.ToString()}}}";
            case KerepTypeCode.HashtablePtr: return $"{{HashtablePtr:{VoidPtr.ToString()}}}";
            default: throw new Exception($"can't unbox value of type {TypeCode}");
        }
    }
}