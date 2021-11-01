using System;
using System.Collections.Generic;
using System.Linq;

namespace DTLib.Reactive
{
    public abstract class ReactiveProvider<T>
    {
        protected List<ReactiveStream<T>> Streams
        {
            get
            { lock (_streams) return _streams; }
            set
            { lock (_streams) _streams = value; }
        }
        private List<ReactiveStream<T>> _streams = new();

        public ReactiveProvider() { }
        public ReactiveProvider(ReactiveStream<T> stream) => Streams.Add(stream);
        public ReactiveProvider(ICollection<ReactiveStream<T>> streams) => Streams = streams.ToList();

        public virtual void Join(ReactiveStream<T> stream)
        {
            if (IsConnetcedTo(stream)) throw new Exception("ReactiveListener is already connected to the stream");
            Streams.Add(stream);
        }

        public virtual void Leave(ReactiveStream<T> stream)
        {
            if (!Streams.Remove(stream)) throw new Exception("ReactiveListener is not connected to the stream");
        }

        public bool IsConnetcedTo(ReactiveStream<T> stream) => Streams.Contains(stream);
    }
}
