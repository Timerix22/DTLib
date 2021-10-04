using DTLib.Filesystem;

namespace DTLib.ConsoleGUI
{
    public class Label : IDrawable
    {
        public (ushort x, ushort y) AnchorPoint { get; set; } = (0, 0);
        public ushort Width { get; private set; }
        public ushort Height { get; private set; }
        public char[] Textmap { get; private set; }
        public char[] Colormap { get; private set; }

        public string TextmapFile { get; set; }
        public string ColormapFile { get; set; }
        public string Name { get; init; }

        public Label() { }

        public Label(string name, string textmapFile, string colormapFile)
        {
            TextmapFile=textmapFile;
            ColormapFile=colormapFile;
            Name=name;
        }

        public void GenColormap() => Colormap=File.ReadAllText(ColormapFile).ToCharArray();

        public void GenTextmap()
        {
            Textmap=File.ReadAllText(TextmapFile).ToCharArray();
            Width=12;
            Height=3;
        }
    }
}
