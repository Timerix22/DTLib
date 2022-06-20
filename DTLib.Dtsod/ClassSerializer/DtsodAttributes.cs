using System.Xml.Serialization;

namespace DTLib.Dtsod;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public class DtsodSerializableAttribute : Attribute
{
    public DtsodVersion Version;
    public DtsodSerializableAttribute(DtsodVersion ver) => Version = ver;
}


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SerializeAsAttribute: Attribute
{
    public string Key;
    /// <summary>
    /// use it only for base types
    /// </summary>
    /// <param name="key">name the field will be serialized as</param>
    public SerializeAsAttribute(string key) => Key = key;
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SerializationMethodAttribute<TSource,TSerialized> : Attribute
{
    public Func<TSource, TSerialized> Method;
    /// <summary>
    /// </summary>
    /// <param name="method">how to serialize field</param>
    public SerializationMethodAttribute(Func<TSource, TSerialized> method) => Method = method;
}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DeserializationMethodAttribute<TSource,TSerialized> : Attribute
{
    public Func<TSerialized, TSource> Method;
    /// <summary>
    /// </summary>
    /// <param name="method">how to deserialize field</param>
    public DeserializationMethodAttribute( Func<TSerialized, TSource> method) => Method = method;
}