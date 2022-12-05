namespace DTLib.Logging.New;

public class DefaultLogFormat : ILogFormat
{

    public bool PrintTimeStamp { get; set; }
    public bool PrintContext { get; set; }
    public bool PrintSeverity { get; set; }

    public DefaultLogFormat(bool printTimeStamp = false, bool printContext = true, bool printSeverity = true)
    {
        PrintTimeStamp = printTimeStamp;
        PrintContext = printContext;
        PrintSeverity = printSeverity;
    }
    
    public string CreateMessage(string context, LogSeverity severity, object message)
    {
        var sb = new StringBuilder();
        if (PrintTimeStamp) 
            sb.Append('[').Append(DateTime.Now.ToString(MyTimeFormat.ForText)).Append(']');
        if (PrintContext && PrintSeverity) 
            sb.Append('[').Append(context).Append('/').Append(severity.ToString()).Append(']');
        else if(PrintContext) 
            sb.Append('[').Append(context).Append(']');
        else if(PrintSeverity) 
            sb.Append('[').Append(severity.ToString()).Append(']');
        if (sb.Length != 0) 
            sb.Append(": ");
        sb.Append(message.ToString());
        sb.Append('\n');
        return sb.ToString();
    }
}