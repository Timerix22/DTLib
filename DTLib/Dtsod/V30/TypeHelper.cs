namespace DTLib.Dtsod;

public class TypeHelper
{
    static Lazy<TypeHelper> _inst = new();
    public static TypeHelper Instance => _inst.Value;

    internal readonly Dictionary<Type, Func<string, dynamic>> BaseTypeConstructors = new()
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

    internal Dictionary<Type, string> BaseTypeNames = new()
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

    private DtsodDict<string, Type> ST_extensions = new()
    {
        { "List<bool>", typeof(List<bool>) },
        { "List<char>", typeof(List<char>) },
        { "List<string>", typeof(List<string>) },
        { "List<byte>", typeof(List<byte>) },
        { "List<sbyte>", typeof(List<sbyte>) },
        { "List<short>", typeof(List<short>) },
        { "List<ushort>", typeof(List<ushort>) },
        { "List<int>", typeof(List<int>) },
        { "List<uint>", typeof(List<uint>) },
        { "List<long>", typeof(List<long>) },
        { "List<ulong>", typeof(List<ulong>) },
        { "List<float>", typeof(List<float>) },
        { "List<double>", typeof(List<double>) },
        { "List<decimal>", typeof(List<decimal>) },
    };
    private DtsodDict<Type, string> TS_extensions = new()
    {
        { typeof(List<bool>), "List<bool>" },
        { typeof(List<char>), "List<char>" },
        { typeof(List<string>), "List<string>" },
        { typeof(List<byte>), "List<byte>" },
        { typeof(List<sbyte>), "List<sbyte>" },
        { typeof(List<short>), "List<short>" },
        { typeof(List<ushort>), "List<ushort>" },
        { typeof(List<int>), "List<int>" },
        { typeof(List<uint>), "List<uint>" },
        { typeof(List<long>), "List<long>" },
        { typeof(List<ulong>), "List<ulong>" },
        { typeof(List<float>), "List<float>" },
        { typeof(List<double>), "List<double>" },
        { typeof(List<decimal>), "List<decimal>" },
    };

    public TypeHelper Extend(string name, Type type)
    {
        ST_extensions.Add(name, type);
        TS_extensions.Add(type, name);
        return this; 
    }

    public string TypeToString(Type t) =>
        BaseTypeNames.TryGetValue(t, out string name)
            ? name
            : TS_extensions.TryGetValue(t, out name)
                ? name
                : t.FullName;
    public Type TypeFromString(string str) => str switch
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
        _ => ST_extensions.TryGetValue(str, out var t) 
                ? t
                : Type.GetType(str, false) 
                    ?? throw new Exception($"DtsodV30.Deserialize.ParseType() error: type {str} doesn't exists")
    };
    internal static T As<T>(object inst) where T : class => inst as T;
}
