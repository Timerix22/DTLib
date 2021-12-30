﻿namespace DTLib;

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

    // вывод цветного текста
    public static void Write(params string[] input)
    {
        if (input.Length == 1)
        {
            if (Console.ForegroundColor != ConsoleColor.Gray)
                Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(input[0]);
        }
        else if (input.Length % 2 == 0)
        {
            StringBuilder strB = new();
            for (ushort i = 0; i < input.Length; i++)
            {
                ConsoleColor c = ParseColor(input[i]);
                if (Console.ForegroundColor != c)
                {
                    Console.Write(strB.ToString());
                    Console.ForegroundColor = c;
                    strB.Clear();
                }
                strB.Append(input[++i]);
            }
            if (strB.Length > 0)
                Console.Write(strB.ToString());
        }
        else throw new Exception("ColoredConsole.Write() error: every text string must have color string before");
    }

    // ввод цветного текста
    public static string Read(string color)
    {
        ConsoleColor c = ParseColor(color);
        if (Console.ForegroundColor != c)
            Console.ForegroundColor = c;
        return Console.ReadLine();
    }
}
