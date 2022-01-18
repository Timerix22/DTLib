using System;
using System.Collections;
using System.Collections.Generic;

namespace DTLib.Reactive
{
    public class ReactiveStream<T> : IEnumerable<TimeSignedObject<T>>, IList<TimeSignedObject<T>>
    {
        public ReactiveStream() { }

        List<TimeSignedObject<T>> _storage = new();
        List<TimeSignedObject<T>> Storage
        {
            get
            { lock (_storage) return _storage; }
        }

        public int Count => Storage.Count;

        public TimeSignedObject<T> this[int index]
        {
            get => Storage[index];
            set => throw new NotImplementedException();
        }

        public event Action<ReactiveStream<T>, TimeSignedObject<T>> ElementAddedEvent;
        public void Add(TimeSignedObject<T> elem)
        {
            Storage.Add(elem);
            ElementAddedEvent?.Invoke(this, elem);
        }
        public void Add(T elem) => Add(new TimeSignedObject<T>(elem));

        public void Clear() => Storage.Clear();
        public int IndexOf(TimeSignedObject<T> item) => Storage.IndexOf(item);
        public bool Contains(TimeSignedObject<T> item) => Storage.Contains(item);

        public IEnumerator<TimeSignedObject<T>> GetEnumerator() => new Enumerator(Storage);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Storage);

        struct Enumerator : IEnumerator<TimeSignedObject<T>>
        {
            public Enumerator(List<TimeSignedObject<T>> storage)
            {
                _storage = storage;
                _index = storage.Count - 1;
            }

            List<TimeSignedObject<T>> _storage;
            int _index;
            public TimeSignedObject<T> Current => _storage[_index];
            object IEnumerator.Current => Current;

            public void Dispose() => _storage = null;

            public bool MoveNext()
            {
                if (_index < 0)
                    return false;
                _index--;
                return true;
            }

            public void Reset() => _index = _storage.Count - 1;
        }

        bool ICollection<TimeSignedObject<T>>.IsReadOnly { get; } = false;

        public void Insert(int index, TimeSignedObject<T> item) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();
        public void CopyTo(TimeSignedObject<T>[] array, int arrayIndex) => throw new NotImplementedException();
        public bool Remove(TimeSignedObject<T> item) => throw new NotImplementedException();
    }
}