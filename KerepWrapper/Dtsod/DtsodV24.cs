using System;
using System.Collections;
using System.Collections.Generic;
using DTLib.Dtsod;
using DTLib.Extensions;
using KerepWrapper.KerepTypes;
using KerepWrapper.Autoarr;
using Funcs=KerepWrapper.Dtsod.DtsodV24Functions;

namespace KerepWrapper.Dtsod;

public class DtsodV24 : IDtsod, IEnumerable<KVPair>, IDisposable
{
    public DtsodVersion Version => DtsodVersion.V24;
    public readonly DtsodPtr UnmanagedPtr;
    //if true, destructor frees allocated unmanaged memory
    public bool AutoDispose = true;
    public ushort Height => Funcs.Height(UnmanagedPtr);
    
    public DtsodV24(DtsodPtr ptr) => UnmanagedPtr = ptr;
    public DtsodV24(string text, bool autoDispose = true) : this(Funcs.Deserialize(text))
        => AutoDispose = autoDispose;
    public DtsodV24(bool autoDispose=true) : this(" ", autoDispose) { }

    public DtsodV24(IDictionary<string,dynamic> dict, bool autoDispose=true) : this(autoDispose)
    {
        foreach (KeyValuePair<string, dynamic> pair in dict)
        {
            if (pair.Value is not null) AddOrSet(pair.Key, pair.Value);
            //else Log("y", $"skiping key <{pair.Key}> with null value");
        }
    }

    public IDictionary<string, dynamic> ToDictionary()
    {
        Dictionary<string, dynamic> dict = new();
        foreach (var p in this)
                dict.Add(p.key.HGlobalUTF8ToString(),p.value.ToDynamic());
        return dict;
    }


    public bool TryGet(string key, out dynamic elem)
    {
        var g = Funcs.Get(UnmanagedPtr, key);
        elem = g.ToDynamic();
        return g.TypeCode == KerepTypeCode.Null;
    }

    public void AddOrSet(string key, dynamic value) =>
        Funcs.AddOrSet(UnmanagedPtr, key, new Unitype(value));
    
    public dynamic this[string key]
    {
        get
        {
            if (!TryGet(key, out var v)) throw new KeyNotFoundException($"key <{key}> not found");
            return v;
        }
        set => AddOrSet(key, value);
    }

    public bool TryRemove(string key) => Funcs.Remove(UnmanagedPtr,key);

    [Obsolete("do you really need to use this? look at TryGet/TryRemove/AddOrSet")]
    public bool ContainsKey(string key) => Funcs.Contains(UnmanagedPtr, key);

    public override string ToString() => Funcs.Serialize(UnmanagedPtr);

    public void Dispose() => Funcs.Free(UnmanagedPtr);
    
    ~DtsodV24()
    {
        if(AutoDispose) Dispose();
    }


    public IEnumerator<KVPair> GetEnumerator() => new DtsodV24Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    class DtsodV24Enumerator: IEnumerator<KVPair>
    {
        private readonly DtsodV24 d;
        private ushort h;
        private IEnumerator<KVPair> arEnumerator;
        
        public DtsodV24Enumerator(DtsodV24 _d) => d = _d;

        bool NextAr()
        {
            if (h >= Funcs.Height(d.UnmanagedPtr)) return false;
            var ar = new Autoarr<KVPair>(Funcs.GetRow(d.UnmanagedPtr, h), false);
            arEnumerator = ar.GetEnumerator();
            h++;
            return true;
        }
        
        public bool MoveNext()
        {
            if(arEnumerator==null)
                NextAr();
            while(!arEnumerator.MoveNext()) 
                if(!NextAr()) return false;
            Current = arEnumerator.Current;
            return true;
        }

        public void Reset() => h = 0;
        public KVPair Current { get; private set; }
        object IEnumerator.Current => Current;
        
        public void Dispose() { }
    }
}