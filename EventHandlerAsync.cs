using System.Threading.Tasks;

namespace DTLib
{
    // по идее это нужно, чтоб делать так: SomeEvent?.Invoke().Wait()
    public delegate Task EventHandlerAsyncDelegate();
    public delegate Task EventHandlerAsyncDelegate<T>(T e);
    public delegate Task EventHandlerAsyncDelegate<T0, T1>(T0 e0, T1 e1);
    public delegate Task EventHandlerAsyncDelegate<T0, T1, T2>(T0 e0, T1 e1, T2 e2);
    public delegate Task EventHandlerAsyncDelegate<T0, T1, T2, T3>(T0 e0, T1 e1, T2 e2, T3 e3);
}
