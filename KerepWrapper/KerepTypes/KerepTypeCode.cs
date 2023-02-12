using System;
using System.Collections.Generic;

namespace KerepWrapper.KerepTypes;

public enum KerepTypeCode : byte
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

public static class KerepTypeHelper
{
    
    static readonly Dictionary<Type, KerepTypeCode> type_comparsion_dict = new()
    {
        {typeof(bool), KerepTypeCode.Bool},
        {typeof(byte), KerepTypeCode.UInt8},
        {typeof(ushort), KerepTypeCode.UInt16},
        {typeof(uint), KerepTypeCode.UInt32},
        {typeof(ulong), KerepTypeCode.UInt64},
        {typeof(sbyte), KerepTypeCode.Int8},
        {typeof(short), KerepTypeCode.Int16},
        {typeof(int), KerepTypeCode.Int32},
        {typeof(long), KerepTypeCode.Int64},
        {typeof(float), KerepTypeCode.Float32},
        {typeof(double), KerepTypeCode.Float64},
        {typeof(string), KerepTypeCode.CharPtr}
    };

    public static KerepTypeCode GetKerepTypeCode(object something)
    {
        if (type_comparsion_dict.TryGetValue(something.GetType(), out var ktype))
            return ktype;
        
        else return something switch
        {
            IList<object> => KerepTypeCode.AutoarrUnitypePtr,
            IDictionary<string, object> => KerepTypeCode.HashtablePtr,
            _ => throw new Exception($"can't get KerepTypeCode for type {something.GetType()}")
        };
    }
}