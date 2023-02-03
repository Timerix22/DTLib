namespace DTLib.Console;

#nullable enable
public class LaunchArgument
{
    public string[] Aliases;
    public string Description;
    public string? ParamName;
    public Action? Handler;
    public Action<string>? HandlerWithArg;
    public int Priority;
    
    private LaunchArgument(string[] aliases, string description, int priority)
    {
        Aliases = aliases;
        Description = description;
        Priority = priority;
    }
    
    public LaunchArgument(string[] aliases, string description, Action handler, int priority=0) 
        : this(aliases, description, priority) => Handler = handler;

    public LaunchArgument(string[] aliases, string description, Action<string> handler, string paramName, int priority=0)
        : this(aliases, description, priority)
    {
        HandlerWithArg = handler;
        ParamName = paramName;
    }

    public StringBuilder AppendHelpInfo(StringBuilder b)
    {
        b.Append(Aliases[0]);
        for (int i = 1; i < Aliases.Length; i++)
            b.Append(", ").Append(Aliases[i]);
        if (!String.IsNullOrEmpty(ParamName))
            b.Append(" [").Append(ParamName).Append(']');
        b.Append(" - ").Append(Description);
        return b;
    }

    public override string ToString() => 
        $"{{{{{Aliases.MergeToString(", ")}}}, Handler: {Handler is null}, HandlerWithArg: {HandlerWithArg is null}}}";
}