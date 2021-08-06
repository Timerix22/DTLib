using System.Collections.Generic;

namespace DTLib.ConsoleGUI
{
    public class Container : List<IDrawable>, IDrawable
    {
        public (ushort x, ushort y) AnchorPoint { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public char[] Textmap { get; private set; }
        public char[] Colormap { get; private set; }

        public Container() { }

        public Container(ushort width, ushort height)
        {
            Width = width;
            Height = height;
        }

        public void GenTextmap()
        {
            Textmap = new char[Width * Height];
            for (int i = 0; i < Textmap.Length; i++)
                Textmap[i] = ' ';
            foreach (var element in this)
            {
                for (ushort y = 0; y < element.Height; y++)
                    for (ushort x = 0; x < element.Width; x++)
                    {
                        element.GenTextmap();
                        Textmap[(element.AnchorPoint.y + y) * Width + element.AnchorPoint.x + x] = element.Textmap[y * element.Width + x];
                    }
            }
        }

        public void GenColormap()
        {

        }
    }
}
