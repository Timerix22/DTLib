using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public class ReactiveListener<T> : ReactiveProvider<T>
    {
        public ReactiveListener() { }
        public ReactiveListener(ReactiveStream<T> stream) : base(stream) { }
        public ReactiveListener(ICollection<ReactiveStream<T>> streams) : base(streams) { }


        public event Action<ReactiveStream<T>, T> ElementAddedEvent;
        void ElementAdded(ReactiveStream<T> stream, TimeSignedObject<T> e) => ElementAdded(stream, e.Value);
        void ElementAdded(ReactiveStream<T> stream, T e) =>
            Task.Run(() => ElementAddedEvent?.Invoke(stream, e));

        public override void Join(ReactiveStream<T> stream)
        {
            base.Join(stream);
            lock (Streams) stream.ElementAddedEvent += ElementAdded;
        }

        public override void Leave(ReactiveStream<T> stream)
        {
            base.Leave(stream);
            lock (Streams) stream.ElementAddedEvent -= ElementAdded;
        }

        public T GetFirst()
        {
            if (Streams.Count == 0) throw new Exception("ReactiveListener is not connected to any streams");
            TimeSignedObject<T> rezult = null;
            foreach (ReactiveStream<T> stream in Streams)
                if (stream.Count != 0)
                {
                    TimeSignedObject<T> e = stream[0];
                    if (rezult is null) rezult = e;
                    else if (rezult.Time > e.Time) rezult = e;
                }
            return rezult.Value;
        }
        public T GetLast()
        {
            if (Streams.Count == 0) throw new Exception("ReactiveListener is not connected to any streams");
            TimeSignedObject<T> rezult = null;
            foreach (ReactiveStream<T> stream in Streams)
                if (stream.Count != 0)
                {
                    TimeSignedObject<T> e = stream[stream.Count - 1];
                    if (rezult is null) rezult = e;
                    else if (rezult.Time < e.Time) rezult = e;
                }
            return rezult.Value;
        }

        public T FindOne(Func<T, bool> condition)
        {
            if (Streams.Count == 0) throw new Exception("ReactiveListener is not connected to any streams");
            foreach (ReactiveStream<T> stream in Streams)
                foreach (TimeSignedObject<T> el in stream)
                    if (condition(el.Value))
                        return el.Value;
            return default;
        }

        public TimeSignedObject<T> FindOne(Func<TimeSignedObject<T>, bool> condition)
        {
            if (Streams.Count == 0) throw new Exception("ReactiveListener is not connected to any streams");
            foreach (ReactiveStream<T> stream in Streams)
                foreach (TimeSignedObject<T> el in stream)
                    if (condition(el))
                        return el;
            return default;
        }

        public List<T> FindAll(Func<T, bool> condition)
        {
            if (Streams.Count == 0) throw new Exception("ReactiveListener is not connected to any streams");
            List<T> rezults = new();
            foreach (ReactiveStream<T> stream in Streams)
                foreach (TimeSignedObject<T> el in stream)
                    if (condition(el.Value))
                        rezults.Add(el.Value);
            return rezults;
        }

        public List<TimeSignedObject<T>> FindAll(Func<TimeSignedObject<T>, bool> condition)
        {
            if (Streams.Count == 0) throw new Exception("ReactiveListener is not connected to any streams");
            List<TimeSignedObject<T>> rezults = new();
            foreach (ReactiveStream<T> stream in Streams)
                foreach (TimeSignedObject<T> el in stream)
                    if (condition(el))
                        rezults.Add(el);
            return rezults;
        }
    }
}
