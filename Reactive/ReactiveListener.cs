using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public class ReactiveListener<T> : ReactiveWorker<T>
    {
        public ReactiveListener() { }
        public ReactiveListener(ReactiveStream<T> stream) : base(stream) { }
        public ReactiveListener(IEnumerable<ReactiveStream<T>> streams) : base(streams) { }

        public Action<object, T> ElementAddedHandler;
        public void SetHandler(Action<object, T> handler) => ReactiveWorkerMutex.Execute(() => ElementAddedHandler=handler);
        public async Task ElementAdded(object s, T e) => await Task.Run(() => ElementAddedHandler?.Invoke(s, e));

        public override void Join(ReactiveStream<T> stream) =>
            ReactiveWorkerMutex.Execute(() =>
            {
                Streams.Add(stream);
                stream.ElementAdded+=ElementAdded;
            });

        public override void Leave(ReactiveStream<T> stream) =>
            ReactiveWorkerMutex.Execute(() =>
            {
                Streams.Remove(stream);
                stream.ElementAdded-=ElementAdded;
            });
    }
}
