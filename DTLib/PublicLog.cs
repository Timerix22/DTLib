namespace DTLib;

//
// вывод логов со всех классов в библиотеке
//
public static class PublicLog
{
    public delegate void LogDelegate(params string[] msg);
    // вот к этому объекту подключайте методы для вывода логов
    public static event LogDelegate LogEvent;
    public static void Log(params string[] msg) => LogEvent?.Invoke(msg);

    public static event LogDelegate LogNoTimeEvent;
    public static void LogNoTime(params string[] msg) => LogNoTimeEvent?.Invoke(msg);
}
