using System.Threading;

namespace DTLib;

// 
// простой и понятный класс для выполнения каких-либо действий в отдельном потоке раз в некоторое время
//
public class Timer
{
    Task TimerTask;
    bool Repeat;
    CancellationTokenSource кансель = new();

    // таймер сразу запускается
    public Timer(bool repeat, int delay, Action method)
    {
        Repeat = repeat;
        TimerTask = new Task(() =>
          {
              do
              {
                  if (кансель.Token.IsCancellationRequested)
                      return;
                  Task.Delay(delay).Wait();
                  method();
              } while (Repeat);
          });
    }


    public void Start() => TimerTask.Start();

    // завершение потока
    public void Stop()
    {
        Repeat = false;
        кансель.Cancel();
    }
}
