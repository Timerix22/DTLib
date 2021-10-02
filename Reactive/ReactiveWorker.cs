using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public abstract class ReactiveWorker<T>
    {
        List<ReactiveStream<T>> Streams = new();

        SafeMutex StreamCollectionAccess = new();

        public void Join(ReactiveStream<T> stream) => StreamCollectionAccess.Execute(()=>Streams.Add(stream));
        public void Leave(ReactiveStream<T> stream) => StreamCollectionAccess.Execute(() => Streams.Remove(stream));
    }
}
