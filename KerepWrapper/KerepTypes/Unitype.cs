using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DTLib.Extensions;
using KerepWrapper.Autoarr;
using KerepWrapper.Dtsod;

namespace KerepWrapper.KerepTypes;

[StructLayout(LayoutKind.Explicit)]
public struct Unitype
{
    [FieldOffset(0)] public long Int64;
    [FieldOffset(0)] public ulong UInt64;
    [FieldOffset(0)] public double Float64;
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)] public bool Bool;
    [FieldOffset(0)] public DtsodPtr VoidPtr;
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
                VoidPtr = ((string)v).StringToHGlobalUTF8();
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

    public dynamic ToDynamic()
    {
        switch (TypeCode)
        {
            case KerepTypeCode.Null: return null;
            case KerepTypeCode.Bool: return Bool;
            case KerepTypeCode.Int64: return Int64;
            case KerepTypeCode.UInt64: return UInt64;
            case KerepTypeCode.Float64: return Float64;
            case KerepTypeCode.CharPtr: return VoidPtr.HGlobalUTF8ToString();
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
            case KerepTypeCode.CharPtr: return $"{{CharPtr:{Unmanaged.HGlobalUTF8ToString(VoidPtr)}}}";
            case KerepTypeCode.AutoarrUnitypePtr: return $"{{AutoarrUnitypePtr:{VoidPtr.ToString()}}}";
            case KerepTypeCode.AutoarrKVPairPtr: return $"{{AutoarrKVPairPtr:{VoidPtr.ToString()}}}";
            case KerepTypeCode.HashtablePtr: return $"{{HashtablePtr:{VoidPtr.ToString()}}}";
            default: throw new Exception($"can't unbox value of type {TypeCode}");
        }
    }
}