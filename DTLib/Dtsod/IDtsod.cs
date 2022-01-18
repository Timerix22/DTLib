namespace DTLib.Dtsod;

public interface IDtsod
{
    public DtsodVersion Version { get; }

    public IDictionary<string, dynamic> ToDictionary();
}
