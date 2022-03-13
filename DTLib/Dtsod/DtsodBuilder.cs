namespace DTLib.Dtsod;

public class DtsodBuilder
{
    private StringBuilder b = new();
    
    public DtsodBuilder AddProperty(string key, dynamic value)
    {
        
        return this;
    }


    public string BuildToString() => b.ToString();

    public DtsodV23 Build() => new DtsodV23(BuildToString());
}