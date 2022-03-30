using DTLib.Dtsod.V24.Autoarr;
using Funcs=DTLib.Dtsod.V24.DtsodV24Functions;

namespace DTLib.Dtsod.V24;

public class DtsodV24 : IDtsod, IEnumerable<Autoarr<KVPair>>, IDisposable
{
    public DtsodVersion Version => DtsodVersion.V24;
    private readonly DtsodPtr DtsodHandler;
    //if true, destructor frees allocated unmanaged memory
    public bool AutoDispose = true;

    public DtsodV24(DtsodPtr ptr) => DtsodHandler = ptr;
    
    public DtsodV24() => DtsodHandler = Funcs.Deserialize(" ");

    public DtsodV24(string text) => DtsodHandler = Funcs.Deserialize(text);

    public DtsodV24(IDictionary dict) : this()
    {
        foreach (KeyValuePair<string, dynamic> pair in dict)
            AddOrSet(pair.Key, pair.Value);
    }

    public IDictionary<string, dynamic> ToDictionary()
    {
        DtsodDict<string, dynamic> dict = new();

        return dict;
    }


    public bool TryGet(string key, out dynamic elem)
    {
        var g = Funcs.Get(DtsodHandler, key);
        elem = g.ToDynamic();
        return g.type == my_type.Null;
    }

    public void AddOrSet(string key, dynamic value) =>
        Funcs.AddOrSet(DtsodHandler, key, new Unitype(value));
    
    public dynamic this[string key]
    {
        get
        {
            if (!TryGet(key, out var v)) throw new KeyNotFoundException($"key <{key}> not found");
            return v;
        }
        set => AddOrSet(key, value);
    }

    public bool TryRemove(string key) => Funcs.Remove(DtsodHandler,key);

    [Obsolete("do you really need to use this? look at TryGet/TryRemove/AddOrSet")]
    public bool ContainsKey(string key) => Funcs.Contains(DtsodHandler, key);
    
    public void Dispose() => Funcs.Free(DtsodHandler);
    
    ~DtsodV24()
    {
        if(AutoDispose) Dispose();
    }


    public IEnumerator<Autoarr<KVPair>> GetEnumerator() => new DtsodV24Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    class DtsodV24Enumerator: IEnumerator<Autoarr<KVPair>>
    {
        private readonly DtsodV24 d;
        private ushort h;
        
        public DtsodV24Enumerator(DtsodV24 _d) => d = _d;
        
        public void Dispose() { }

        public bool MoveNext()
        {
            bool r = ++h < Funcs.Height(d.DtsodHandler);
            if(r) Current = new Autoarr<KVPair>(Funcs.GetRow(d.DtsodHandler,h), false);
            return r;
        }

        public void Reset() => h = 0;

        public Autoarr<KVPair> Current { get; private set; }

        object IEnumerator.Current => Current;
    }
}