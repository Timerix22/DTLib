using System.Collections.Generic;

namespace DTLib.Reactive
{
    public abstract class ReactiveWorker<T>
    {
        protected List<ReactiveStream<T>> Streams = new();
        protected SafeMutex ReactiveWorkerMutex = new();

        public ReactiveWorker() { }

        public ReactiveWorker(ReactiveStream<T> stream) => Join(stream);

        public ReactiveWorker(IEnumerable<ReactiveStream<T>> streams) =>
            ReactiveWorkerMutex.Execute(() => { foreach (var stream in streams) Join(stream); });

        public abstract void Join(ReactiveStream<T> stream);
        public abstract void Leave(ReactiveStream<T> stream);
    }
}
