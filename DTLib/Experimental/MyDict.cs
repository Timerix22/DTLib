using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DTLib
{
    public class MyDict<TKey, TVal>
    {
        object locker = new object();
        List<TVal> values;
        List<TKey> keys;
        List<int> hashes;
        int count;

        public int Count
        {
            get
            {
                // lock (count)
                lock (locker) return count;
            }
        }

        public ReadOnlyCollection<TVal> Values
        {
            get
            {
                ReadOnlyCollectionBuilder<TVal> b;
                lock (locker) b = new(values);
                return b.ToReadOnlyCollection();
            }
        }

        public ReadOnlyCollection<TKey> Keys
        {
            get
            {
                ReadOnlyCollectionBuilder<TKey> b;
                lock (locker) b = new(keys);
                return b.ToReadOnlyCollection();
            }
        }

        public MyDict()
        {

            values = new();
            keys = new();
            hashes = new();
            count = 0;
        }

        public MyDict(IList<TKey> _keys, IList<TVal> _values)
        {
            if (_keys.Count != _values.Count) throw new Exception("_keys.Count != _values.Count");
            keys = (List<TKey>)_keys;
            values = (List<TVal>)_values;
            count = _keys.Count;
            hashes = new();
            for (int i = 0; i < count; i++)
                hashes.Add(keys[i].GetHashCode());
        }

        public TVal this[TKey key]
        {
            get
            {
                lock (locker) return values[hashes.IndexOf(key.GetHashCode())];
            }
            set
            {
                lock (locker) values[hashes.IndexOf(key.GetHashCode())] = value;
            }
        }

        public (TKey, TVal) GetByIndex(int index)
        {
            (TKey k, TVal v) output;
            lock (locker)
            {
                output.k = keys[index];
                output.v = values[index];
            }
            return output;
        }

        public void Add(TKey key, TVal val)
        {
            // lock (keys) lock (values) lock (count)
            lock (locker)
            {
                keys.Add(key);
                values.Add(val);
                hashes.Add(key.GetHashCode());
                count++;
            }
        }

        public void Remove(TKey key)
        {
            var hash = key.GetHashCode();
            lock (locker)
            {
                var num = hashes.IndexOf(hash);
                keys.RemoveAt(num);
                values.RemoveAt(num);
                hashes.RemoveAt(num);
                count--;
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                hashes.Clear();
                keys.Clear();
                values.Clear();
                count = 0;
            }
        }
    }
}
