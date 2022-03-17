namespace DTLib.Dtsod;

public class DtsodDict<TKey, TVal> : IDictionary<TKey, TVal>, IDictionary
{
    // да, вместо собственной реализации интерфейса это ссылки на Dictionary
    readonly Dictionary<TKey, TVal> baseDict;

    public DtsodDict() => baseDict = new();
    public DtsodDict(IDictionary<TKey, TVal> srcDict) => baseDict = new(srcDict);


    public virtual TVal this[TKey key]
    {
        get => TryGetValue(key, out TVal value) ? value : throw new Exception($"Dtsod[{key}] key not found");
        set
        {
            if (!TrySetValue(key, value)) throw new KeyNotFoundException($"DtsodDict[{key}] key not found");
        }
    }

    public virtual bool TryGetValue(TKey key, out TVal value) => baseDict.TryGetValue(key, out value);

    public virtual bool TrySetValue(TKey key, TVal value)
    {
        if (ContainsKey(key))
        {
            baseDict[key] = value;
            return true;
        }
        else return false;
    }
    public virtual void Append(ICollection<KeyValuePair<TKey, TVal>> anotherDtsod)
    {
        foreach (KeyValuePair<TKey, TVal> pair in anotherDtsod)
            Add(pair.Key, pair.Value);
    }

    public virtual void Add(TKey key, TVal value) => baseDict.Add(key, value);

    public virtual void Add(KeyValuePair<TKey, TVal> pair)
       => ((ICollection<KeyValuePair<TKey, TVal>>)baseDict).Add(pair);


    public void CopyTo(Array array, int index)
    {
        ((ICollection) baseDict).CopyTo(array, index);
    }

    public int Count => baseDict.Count;
    public bool IsSynchronized => ((ICollection) baseDict).IsSynchronized;

    public object SyncRoot => ((ICollection) baseDict).SyncRoot;

    public ICollection<TKey> Keys => baseDict.Keys;
    ICollection IDictionary.Values => ((IDictionary) baseDict).Values;

    ICollection IDictionary.Keys => ((IDictionary) baseDict).Keys;

    public ICollection<TVal> Values => baseDict.Values;
    public bool IsReadOnly { get; } = false;
    public object this[object key]
    {
        get => ((IDictionary) baseDict)[key];
        set => ((IDictionary) baseDict)[key] = value;
    }

    public void Add(object key, object value)
    {
        ((IDictionary) baseDict).Add(key, value);
    }

    public virtual void Clear() => baseDict.Clear();
    public bool Contains(object key)
    {
        return ((IDictionary) baseDict).Contains(key);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return ((IDictionary) baseDict).GetEnumerator();
    }

    public void Remove(object key)
    {
        ((IDictionary) baseDict).Remove(key);
    }

    public bool IsFixedSize => ((IDictionary) baseDict).IsFixedSize;

    public virtual bool ContainsKey(TKey key) => baseDict.ContainsKey(key);

    bool ICollection<KeyValuePair<TKey, TVal>>.Contains(KeyValuePair<TKey, TVal> pair)
        => ((ICollection<KeyValuePair<TKey, TVal>>)baseDict).Contains(pair);

    void ICollection<KeyValuePair<TKey, TVal>>.CopyTo(KeyValuePair<TKey, TVal>[] array, int arrayIndex)
        => ((ICollection<KeyValuePair<TKey, TVal>>)baseDict).CopyTo(array, arrayIndex);

    public virtual bool Remove(TKey key) => baseDict.Remove(key);
    bool ICollection<KeyValuePair<TKey, TVal>>.Remove(KeyValuePair<TKey, TVal> pair)
        => ((ICollection<KeyValuePair<TKey, TVal>>)baseDict).Remove(pair);

    IEnumerator IEnumerable.GetEnumerator() => baseDict.GetEnumerator();
    public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator() => baseDict.GetEnumerator();
}
