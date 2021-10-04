using System.Collections.Generic;

namespace DTLib.Reactive
{
    public class ReactiveStream<T>
    {
        List<T> Storage = new();
        public event EventHandlerAsync<T> ElementAdded;
        SafeMutex StorageMutex = new();
        public int Length => StorageMutex.Execute(() => Storage.Count);

        public ReactiveStream() { }

        public void Add(T elem)
        {
            StorageMutex.Execute(() => Storage.Add(elem));
            ElementAdded?.Invoke(this, elem);
        }

        public void Get(int index) => StorageMutex.Execute(() => Storage[index]);

        public void Clear() => StorageMutex.Execute(() => Storage.Clear());
    }
}
