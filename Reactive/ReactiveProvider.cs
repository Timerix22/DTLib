using System;
using System.Collections.Generic;

namespace DTLib.Reactive
{
    public class ReactiveProvider<T> : ReactiveWorker<T>
    {

        public ReactiveProvider() { }
        public ReactiveProvider(ReactiveStream<T> stream) : base(stream) { }
        public ReactiveProvider(IEnumerable<ReactiveStream<T>> streams) : base(streams) { }

        event Action<T> AnnounceEvent;
        public void Announce(T e) => ReactiveWorkerMutex.Execute(() => AnnounceEvent.Invoke(e));

        public override void Join(ReactiveStream<T> stream) => ReactiveWorkerMutex.Execute(() =>
                                                             {
                                                                 Streams.Add(stream);
                                                                 AnnounceEvent+=stream.Add;
                                                             });

        public override void Leave(ReactiveStream<T> stream) => ReactiveWorkerMutex.Execute(() =>
                                                              {
                                                                  Streams.Remove(stream);
                                                                  AnnounceEvent-=stream.Add;
                                                              });
    }
}
