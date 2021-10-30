using System.Collections.Generic;

namespace DTLib.Reactive
{
    public class ReactiveStream<T>
    {
        public ReactiveStream() { }

        List<T> _storage = new();
        List<T> Storage
        {
            get
            { lock (Storage) return _storage; }
        }

        public int Length
        {
            get
            { lock (Storage) return Storage.Count; }
        }

        public T this[int index]
        {
            get
            { lock (Storage) return Storage[index]; }
        }

        internal event EventHandlerAsync<T> ElementAddedEvent;

        internal void Add(T elem)
        {
            lock (Storage) Storage.Add(elem);
            ElementAddedEvent?.Invoke(elem);
        }

        internal void Clear() { lock (Storage) Storage.Clear(); }
    }
}