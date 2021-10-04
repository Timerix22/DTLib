using System;
using System.Collections.Generic;

namespace DTLib.Reactive
{
    public class ReactiveStream<T>
    {
        List<T> Storage = new();
        public event EventHandlerAsync<T> ElementAdded;
        bool StoreData = false;

        SafeMutex StorageMutex = new();

        public ReactiveStream() { }
        public ReactiveStream(bool storeData) => StoreData = storeData;

        public void Add(T elem)
        {
            if (StoreData) StorageMutex.Execute(() => Storage.Add(elem));
            ElementAdded?.Invoke(this, elem);
        }

        public void Clear()
        {
            if (StoreData) StorageMutex.Execute(() => Storage.Clear());
            else throw new Exception("Can't clear ReactiveStream because StoreData==false");
        }
    }
}
