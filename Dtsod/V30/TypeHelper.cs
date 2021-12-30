namespace DTLib.Dtsod;

public static class TypeHelper
{
    static public readonly Dictionary<Type, Func<string, dynamic>> BaseTypeConstructors = new()
    {
        { typeof(bool), (inp) => inp.ToBool() },
        { typeof(char), (inp) => inp.ToChar() },
        { typeof(string), (inp) => inp.ToString() },
        { typeof(byte), (inp) => inp.ToByte() },
        { typeof(sbyte), (inp) => inp.ToSByte() },
        { typeof(short), (inp) => inp.ToShort() },
        { typeof(ushort), (inp) => inp.ToUShort() },
        { typeof(int), (inp) => inp.ToInt() },
        { typeof(uint), (inp) => inp.ToUInt() },
        { typeof(long), (inp) => inp.ToLong() },
        { typeof(ulong), (inp) => inp.ToULong() },
        { typeof(float), (inp) => inp.ToFloat() },
        { typeof(double), (inp) => inp.ToDouble() },
        { typeof(decimal), (inp) => inp.ToDecimal() }
    };

    static public Dictionary<Type, string> BaseTypeNames = new()
    {
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" }
    };

    static public string TypeToString(Type t) =>
        BaseTypeNames.TryGetValue(t, out var name)
            ? name
            : t.AssemblyQualifiedName;

    static public Type TypeFromString(string str) => str switch
    {
        "bool" => typeof(bool),
        "char" => typeof(char),
        "string" => typeof(string),
        "byte" => typeof(byte),
        "sbyte" => typeof(sbyte),
        "short" => typeof(short),
        "ushort" => typeof(ushort),
        "int" => typeof(int),
        "uint" => typeof(uint),
        "long" => typeof(long),
        "ulong" => typeof(ulong),
        "float" => typeof(float),
        "double" => typeof(double),
        "decimal" => typeof(decimal),
        _ => Type.GetType(str, false) ??
            throw new Exception($"DtsodV30.Deserialize.ParseType() error: type {str} doesn't exists")
    };

}
