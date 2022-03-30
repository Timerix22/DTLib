using System.Runtime.InteropServices;

namespace DTLib.Dtsod.V24;

internal enum my_type : byte
{
    Null, Float32, Float64, Char, Bool,
    UInt8, Int8, UInt16, Int16, UInt32, Int32, UInt64, Int64, 
    UInt8Ptr, Int8Ptr, UInt16Ptr, Int16Ptr, UInt32Ptr, Int32Ptr, UInt64Ptr, Int64Ptr,
    CharPtr, STNodePtr, HashtablePtr,
    UniversalType,
    AutoarrInt8Ptr, AutoarrUInt8Ptr, AutoarrInt16Ptr, AutoarrUInt16Ptr, 
    AutoarrInt32Ptr, AutoarrUInt32Ptr, AutoarrInt64Ptr, AutoarrUInt64Ptr,
    AutoarrUnitypePtr, AutoarrKVPairPtr
}

[StructLayout(LayoutKind.Explicit)]
internal struct Unitype
{
    [FieldOffset(0)] internal long Int64;
    [FieldOffset(0)] internal ulong UInt64;
    [FieldOffset(0)] internal double Float64;
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)] internal char Char;
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)] internal bool Bool;
    [FieldOffset(0)] internal IntPtr VoidPtr;
    [FieldOffset(8)] internal my_type type;

    public Unitype(dynamic something)
    {
        throw new NotImplementedException();
    }

    public dynamic ToDynamic()
    {
        switch (type)
        {
            case my_type.Null: return null;
            case my_type.Bool: return Bool;
            case my_type.Char: return Char;
            case my_type.Int64: return Int64;
            case my_type.UInt64: return UInt64;
            case my_type.Float64: return Float64;
            case my_type.CharPtr: return Marshal.PtrToStringAuto(VoidPtr);
            case my_type.AutoarrUnitypePtr: return new Autoarr.Autoarr<Unitype>(VoidPtr);
            case my_type.AutoarrKVPairPtr: return new Autoarr.Autoarr<KVPair>(VoidPtr);
            case my_type.HashtablePtr: return new DtsodV24(VoidPtr);
            default: throw new Exception($"can't unbox value of type {type}");
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct KVPair
{
    internal IntPtr key;
    internal Unitype value;
}
