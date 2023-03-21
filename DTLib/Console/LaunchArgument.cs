namespace DTLib.Console;

#nullable enable
public class LaunchArgument
{
    public string[] Aliases;
    public string Description;
    protected string? ParamName1;
    protected string? ParamName2;
    public Action? Handler;
    public Action<string>? HandlerWithArg1;
    public Action<string, string>? HandlerWithArg2;
    public int RequiredArgsCount;
    public int Priority;
    
    private LaunchArgument(string[] aliases, string description, int priority)
    {
        Aliases = aliases;
        Description = description;
        Priority = priority;
    }

    public LaunchArgument(string[] aliases, string description,
        Action handler, int priority = 0)
        : this(aliases, description, priority)
    {
        Handler = handler;
        RequiredArgsCount = 0;
    }

    public LaunchArgument(string[] aliases, string description,
        Action<string> handler, string paramName1, int priority=0)
        : this(aliases, description, priority)
    {
        HandlerWithArg1 = handler;
        ParamName1 = paramName1;
        RequiredArgsCount = 1;
    }
    public LaunchArgument(string[] aliases, string description,
        Action<string, string> handler, string paramName1, string paramName2, int priority=0)
        : this(aliases, description, priority)
    {
        HandlerWithArg2 = handler;
        ParamName1 = paramName1;
        ParamName2 = paramName2;
        RequiredArgsCount = 2;
    }

    public StringBuilder AppendHelpInfo(StringBuilder b)
    {
        b.Append(Aliases[0]);
        for (int i = 1; i < Aliases.Length; i++)
            b.Append(", ").Append(Aliases[i]);
        if (!string.IsNullOrEmpty(ParamName1))
            b.Append(" [").Append(ParamName1).Append("] ");
        if (!string.IsNullOrEmpty(ParamName2))
            b.Append(" [").Append(ParamName2).Append("] ");
        b.Append("- ").Append(Description);
        return b;
    }

    public override string ToString() => 
        $"{{{{{Aliases.MergeToString(", ")}}}, Handler: {Handler is null}, HandlerWithArg: {HandlerWithArg1 is null}}}";
}