namespace DTLib.Reactive
{
    public class ReactiveSender<T> : ReactiveProvider<T>
    {

        public ReactiveSender() { }
        public ReactiveSender(ReactiveStream<T> stream) : base(stream) { }

        public void Send(T e)
        {
            lock (Stream) Stream.Add(e);
        }
    }
}
