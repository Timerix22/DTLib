using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public abstract class ReactiveWorker<T>
    {
        protected List<ReactiveStream<T>> Streams = new();

        protected SafeMutex StreamCollectionAccess = new();

        public void Join(ReactiveStream<T> stream) => StreamCollectionAccess.Execute(()=>Streams.Add(stream));
        public void Leave(ReactiveStream<T> stream) => StreamCollectionAccess.Execute(() => Streams.Remove(stream));
    }
}
