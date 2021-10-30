namespace DTLib.Reactive
{
    public abstract class ReactiveProvider<T>
    {
        protected ReactiveStream<T> Stream
        {
            get
            { lock (_stream) return _stream; }
            set
            { lock (_stream) _stream = value; }
        }
        protected ReactiveStream<T> _stream;

        public ReactiveProvider() { }

        public ReactiveProvider(ReactiveStream<T> stream) => Join(stream);

        public virtual void Join(ReactiveStream<T> stream)
        {
            lock (Stream) Stream = stream;
        }

        public virtual void Leave(ReactiveStream<T> stream)
        {
            lock (Stream) Stream = null;
        }
    }
}
