using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public class ReactiveListener<T> : ReactiveProvider<T>
    {
        public ReactiveListener() { }
        public ReactiveListener(ReactiveStream<T> stream) : base(stream) { }

        public EventHandlerAsync<T> ElementAddedHandler;
        public void SetHandler(EventHandlerAsync<T> handler)
        {
            lock (Stream) ElementAddedHandler = handler;
        }
        public async Task ElementAdded(T e) => await Task.Run(() => ElementAddedHandler?.Invoke(e));

        public override void Join(ReactiveStream<T> stream)
        {
            base.Join(stream);
            lock (Stream) stream.ElementAddedEvent += ElementAdded;
        }

        public override void Leave(ReactiveStream<T> stream)
        {
            base.Leave(stream);
            lock (Stream) stream.ElementAddedEvent -= ElementAdded;
        }

        public T GetFirstElement() => Stream[0];
        public T GetLastElement() => Stream[Stream.Length - 1];

        public T FindOne(Func<T, bool> condition) =>
            /*foreach (T el in Stream)
if (condition(el))
return el;*/
            default;

        public List<T> FindAll(Func<T, bool> condition)
        {
            List<T> elements = new();
            /*foreach (T el in Stream)
                if (condition(el))
                    elements.Add(el);*/
            return elements;
        }
    }
}
