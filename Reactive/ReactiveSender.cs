using System.Collections.Generic;

namespace DTLib.Reactive
{
    public class ReactiveSender<T> : ReactiveProvider<T>
    {

        public ReactiveSender() { }
        public ReactiveSender(ReactiveStream<T> stream) : base(stream) { }
        public ReactiveSender(ICollection<ReactiveStream<T>> streams) : base(streams) { }

        public void Send(T e)
        {
            foreach (ReactiveStream<T> s in Streams)
                s.Add(e);
        }
    }
}
