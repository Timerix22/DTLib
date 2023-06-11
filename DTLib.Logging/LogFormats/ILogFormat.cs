namespace DTLib.Logging;

public interface ILogFormat
{
    bool PrintTimeStamp { get; set; }
    bool PrintContext { get; set; }
    bool PrintSeverity { get; set; }
    
    string CreateMessage(string context, LogSeverity severity, object message);
}