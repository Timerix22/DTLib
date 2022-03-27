using System.Runtime.InteropServices;

namespace DTLib.Dtsod.V24;

internal enum my_type : byte
{
    Null, Float, Double, Char, Bool,
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
    [FieldOffset(0)] internal double Double;
    [FieldOffset(0)] internal byte Char;
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)] internal bool Bool;
    [FieldOffset(0)] internal IntPtr VoidPtr;
    [FieldOffset(8)] internal my_type type;
}

[StructLayout(LayoutKind.Sequential)]
internal struct KVPair
{
    internal IntPtr key;
    internal Unitype value;
}
