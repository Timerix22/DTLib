namespace DTLib;

//
// вывод и ввод цветного текста в консоли
// работает медленнее чем хотелось бы
//
public static class ColoredConsole
{
    // парсит название цвета в ConsoleColor
    public static ConsoleColor ParseColor(string color) => color switch
    {
        //case "magneta":
        "m" => ConsoleColor.Magenta,
        //case "green":
        "g" => ConsoleColor.Green,
        //case "red":
        "r" => ConsoleColor.Red,
        //case "yellow":
        "y" => ConsoleColor.Yellow,
        //case "white":
        "w" => ConsoleColor.White,
        //case "blue":
        "b" => ConsoleColor.Blue,
        //case "cyan":
        "c" => ConsoleColor.Cyan,
        //case "h":
        "h" or "gray" => ConsoleColor.Gray,
        //case "black":
        "black" => ConsoleColor.Black,
        _ => throw new Exception($"ColoredConsole.ParseColor({color}) error: incorrect color"),
    };

    public static void Write(ConsoleColor color,string msg)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void Write(string msg) => Write(ConsoleColor.Gray, msg);
    
    // вывод цветного текста
    public static void Write(params string[] input)
    {
        if (input.Length == 1)
        {
            Write(input[0]);
            return;
        }
        
        if (input.Length % 2 != 0)
            throw new Exception("ColoredConsole.Write() error: every text string must have color string before");
        
        for (ushort i = 0; i < input.Length; i++)
        {
            Console.ForegroundColor = ParseColor(input[i++]);
            Console.Write(input[i]);
        }
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void WriteLine() => Console.WriteLine();
    public static void WriteLine(ConsoleColor color,string msg)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void WriteLine(params string[] input)
    {
        Write(input);
        WriteLine();
    }
    
    // ввод цветного текста
    public static string Read(ConsoleColor color)
    {
        Console.ForegroundColor = color;
        var r = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        return r;
    }

    public static string Read(string color) => Read(ParseColor(color));
}
