namespace DTLib.Console;

public class LaunchArgumentParser
{
    private Dictionary<string, LaunchArgument> argDict = new();
    private List<LaunchArgument> argList = new();
    public bool ExitIfNoArgs = true;

    public class ExitAfterHelpException : Exception
    {
        internal ExitAfterHelpException() : base("your program can use this exception to exit after displaying help message")
        { }
    }
    
    public string CreateHelpMessage()
    {
        StringBuilder b = new();
        foreach (var arg in argList) 
            arg.AppendHelpInfo(b).Append('\n');
        b.Remove(b.Length-1, 1);
        return b.ToString();
    }
    public string CreateHelpArgMessage(string argAlias)
    {
        StringBuilder b = new();
        var arg = Parse(argAlias);
        arg.AppendHelpInfo(b);
        return b.ToString();
    }
    private void HelpHandler()
    {
        System.Console.WriteLine(CreateHelpMessage());
        throw new ExitAfterHelpException();
    }

    private void HelpArgHandler(string argAlias)
    {
        System.Console.WriteLine(CreateHelpArgMessage(argAlias));
        throw new ExitAfterHelpException();
    }


    public LaunchArgumentParser()
    {
        var help = new LaunchArgument(new[] { "h", "help" }, 
            "shows help message", HelpHandler);
        Add(help);
        var helpArg = new LaunchArgument( new[]{ "ha", "helparg" }, 
            "shows help message for particular argument", 
            HelpArgHandler, "argAlias");
        Add(helpArg);
    }

    public LaunchArgumentParser WithNoExit()
    {
        ExitIfNoArgs = false;
        return this;
    }

    public LaunchArgumentParser(ICollection<LaunchArgument> arguments) : this()
    {
        foreach (var arg in arguments) 
            Add(arg);
    }
    public LaunchArgumentParser(params LaunchArgument[] arguments) : this()
    {
        for (var i = 0; i < arguments.Length; i++)
            Add(arguments[i]);
    }

    public void Add(LaunchArgument arg)
    {
        argList.Add(arg);
        for(int a=0; a<arg.Aliases.Length; a++)
            argDict.Add(arg.Aliases[a], arg);
    }
    
    public LaunchArgument Parse(string argAlias)
    {
        // different argument providing patterns
        if (!argDict.TryGetValue(argAlias, out var arg) &&    // arg
            !(argAlias.StartsWith("--") && argDict.TryGetValue(argAlias.Substring(2), out arg)) && // --arg
            !(argAlias.StartsWith('-') && argDict.TryGetValue(argAlias.Substring(1), out arg)) &&  // -arg
            !(argAlias.StartsWith('/') && argDict.TryGetValue(argAlias.Substring(1), out arg)))    // /arg
            throw new Exception($"invalid argument: {argAlias}\n{CreateHelpMessage()}");
        
        return arg;
    }
    
    /// <param name="args">program launch args</param>
    /// <exception cref="Exception">argument {args[i]} should have a parameter after it</exception>
    /// <exception cref="NullReferenceException">argument hasn't got any handlers</exception>
    /// <exception cref="ExitAfterHelpException">happens after help message is displayed</exception>
    public void ParseAndHandle(string[] args)
    {
        // show help and throw
        if (args.Length == 0 && ExitIfNoArgs) 
            HelpHandler();

        List<LaunchArgument> execQueue = new();
        
        for (int i = 0; i < args.Length; i++)
        {
            LaunchArgument arg = Parse(args[i]);

            switch (arg.RequiredArgsCount)
            {
                case 0:
                    if (arg.Handler is null) 
                        throw new NullReferenceException($"argument <{args[i]}> hasn't got any handlers");
                    break;
                case 1:
                {
                    if (arg.HandlerWithArg1 is null)
                        throw new NullReferenceException($"argument <{args[i]}> hasn't got any handlers");
                    if (i + 1 >= args.Length)
                        throw new Exception($"argument <{args[i]}> should have a parameter after it");
                    string arg1 = args[++i];
                    arg.Handler = () => arg.HandlerWithArg1(arg1);
                    break;
                }
                case 2:
                {
                    if (arg.HandlerWithArg2 is null)
                        throw new NullReferenceException($"argument <{args[i]}> hasn't got any handlers");
                    if (i + 2 >= args.Length)
                        throw new Exception($"argument <{args[i]}> should have two params after it");
                    string arg1 = args[++i], arg2 = args[++i];
                    arg.Handler = () => arg.HandlerWithArg2(arg1, arg2);
                    break;
                }
            }
            execQueue.Add(arg);
        }
        
        // ascending sort by priority
        execQueue.Sort((a0, a1) => a0.Priority-a1.Priority);
        // finally executing handlers
        foreach (var a in execQueue) 
            a.Handler!();
    }
}