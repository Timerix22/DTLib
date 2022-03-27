namespace DTLib.Filesystem;

public static class Путь
{
    public static readonly char Разд = Environment.OSVersion.Platform == PlatformID.Win32NT ? '\\' : '/';

    public static string ИсправитьРазд(this string путь)
    {
        if (Разд == '\\')
        {
            if (путь.Contains('/'))
                путь = путь.Replace('/', '\\');
        }
        else
        {
            if (путь.Contains('\\'))
                путь = путь.Replace('\\', '/');
        }
        return путь;
    }

    // replaces wrong characters to use string as путь
    public static string НормализоватьДляПути(this string путь) =>
        путь.Replace(':', '-').Replace(' ', '_');

    public static void Предупредить(string может_быть_с_точками)
    {
        if(может_быть_с_точками.Contains("..")) 
            throw new Exception($"лее точки двойные убери: {может_быть_с_точками}");
    }
}