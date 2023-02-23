namespace DTLib.Dtsod;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class SerializeAttribute : Attribute
{
    /// <summary>
    /// generates serializers for all class fields or adds property to serializer method
    /// </summary>
    public SerializeAttribute() { }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class NotSerializeAttribute : Attribute
{
    public Func<bool> Condition;

    /// <summary>
    /// disables field serialization
    /// </summary>
    public NotSerializeAttribute() { }

    /// <summary>
    /// disables field serialization if condition is satisfied
    /// </summary>
    /// <param name="condition">if true, field won't be serialized</param>
    public NotSerializeAttribute(Func<bool> condition) => 
        Condition = condition;
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SerializeAsAttribute : Attribute
{
    public string CustomFieldName;
    /// <summary>
    /// generates field serialization code 
    /// </summary>
    /// <param name="customFieldName">overrides field name in dtsod</param>
    public SerializeAsAttribute(string customFieldName=null) => 
        CustomFieldName = customFieldName;
}

