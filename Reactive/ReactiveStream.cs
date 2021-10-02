using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public class ReactiveStream<T> 
    {
        List<T> Storage = new();
        public event EventHandlerAsync<T> ElementAdded;
        bool StoreData = false;

        SafeMutex StorageAccess = new();

        public ReactiveStream() { }
        public ReactiveStream(bool storeData) => StoreData = storeData;

        public void Add(T elem)
        {
            if (StoreData) StorageAccess.Execute(()=> Storage.Add(elem));
            ElementAdded?.Invoke(this, elem);
        }
        
    }
}
