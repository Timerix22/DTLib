using System;
using System.Threading;

namespace DTLib
{
    public class SafeMutex
    {
        readonly Mutex Mutex = new();
        bool isReleased = false;

        // тут выполняется отправленный код
        public void Execute(Action action, out Exception exception)
        {
            try
            {
                exception=null;
                Execute(action);
            }
            catch(Exception ex)
            {
                exception=ex;
                if(!isReleased)
                    Mutex.ReleaseMutex();
            }
        }

        public void Execute(Action action)
        {
            Mutex.WaitOne();
            action();
            Mutex.ReleaseMutex();
            isReleased=true;
        }
        public T Execute<T>(Func<T> action)
        {
            Mutex.WaitOne();
            T rezult = action();
            Mutex.ReleaseMutex();
            isReleased=true;
            return rezult;
        }
    }
}
