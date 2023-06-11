namespace DTLib.Logging;

/// <summary>
/// вывод логов со всех классов в библиотеке
/// </summary>
public static class InternalLog
{
    public delegate void LogDelegate(params string[] msg);
    // вот к этому объекту подключайте методы для вывода логов
    public static event LogDelegate LogEvent;
    public static void Log(params string[] msg) => LogEvent?.Invoke(msg);
}
