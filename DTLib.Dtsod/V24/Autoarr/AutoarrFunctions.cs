using DTLib.Dtsod.V24.KerepTypes;

namespace DTLib.Dtsod.V24.Autoarr;
using AutoarrPtr=IntPtr;

public abstract class AutoarrFunctions<T>
{
    static AutoarrFunctions()
    {
        DependencyResolver.CopyLibs();
    }
    
    internal abstract AutoarrPtr Create(ushort maxBlocksCount, ushort maxBlockLength);
    internal abstract void Free(AutoarrPtr ar);
    internal abstract T Get(AutoarrPtr ar, uint index);
    internal abstract void Add(AutoarrPtr ar, T element);
    internal abstract void Set(AutoarrPtr ar, uint index, T element);
    internal abstract uint Length(AutoarrPtr ar);
    internal abstract uint MaxLength(AutoarrPtr ar);

    private static AutoarrFunctions<Unitype> f_uni = new AutoarrUnitypeFunctions();
    private static AutoarrFunctions<KVPair> f_kvp = new AutoarrKVPairFunctions();
    static internal AutoarrFunctions<T> GetFunctions()
    {
        if (f_kvp is AutoarrFunctions<T> f)
            return f;
        else if (f_uni is AutoarrFunctions<T> ff)
            return ff;
        else throw new Exception($"unsupported type: {typeof(T)}");
        /*if (typeof(T) == typeof(Unitype)) 
            return (AutoarrFunctions<T>)Convert.ChangeType(f_uni, typeof(AutoarrFunctions<T>));
        else if (typeof(T) == typeof(KVPair))
            return (AutoarrFunctions<T>) Convert.ChangeType(f_kvp, typeof(AutoarrFunctions<T>));
        else throw new Exception($"unsupported type: {typeof(T)}");*/
    }
}