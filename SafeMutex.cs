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
                exception = null;
                Mutex.WaitOne();
                action();
                Mutex.ReleaseMutex();
                isReleased = true;
            }
            catch (Exception ex)
            {
                exception = ex;
                if (!isReleased) Mutex.ReleaseMutex();
            }
        }
    }
}
