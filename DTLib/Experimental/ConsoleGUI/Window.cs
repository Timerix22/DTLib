using DTLib.Filesystem;
using System;
using System.Text;

namespace DTLib.ConsoleGUI
{
    //
    // создание gui из текста в консоли
    //
    public class Window : Container
    {

        public Window(string layout_file) : base("window", layout_file)
        {
            Console.Clear();
            Console.SetWindowSize(Width+1, Height+1);
            Console.SetBufferSize(Width+1, Height+1);
            Console.OutputEncoding=Encoding.Unicode;
            Console.InputEncoding=Encoding.Unicode;
            Console.CursorVisible=false;
        }

        // выводит все символы
        public void RenderFile(string file)
        {
            Console.Clear();
            Console.WriteLine(File.ReadAllText(file));
        }

        public void Render()
        {
            GenTextmap();
            Console.WriteLine(SimpleConverter.MergeToString(Textmap));
        }
    }
}
