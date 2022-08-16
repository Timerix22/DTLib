namespace DTLib;

public class MyTimeFormat : IFormatProvider
{
    public static MyTimeFormat Instance=new();
    public object GetFormat(Type formatType)
    {
        if(formatType==typeof(DateTime))
            return "yyyy-MM-dd_HH-mm-ss+zz";
        else throw new FormatException();
    }
}