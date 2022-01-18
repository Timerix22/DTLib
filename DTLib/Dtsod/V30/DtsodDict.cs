namespace DTLib.Dtsod;

public class DtsodDict<TKey, TVal> : IDictionary<TKey, TVal>
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


    public int Count => baseDict.Count;
    public ICollection<TKey> Keys => baseDict.Keys;
    public ICollection<TVal> Values => baseDict.Values;
    public bool IsReadOnly { get; } = false;

    public virtual void Clear() => baseDict.Clear();

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
