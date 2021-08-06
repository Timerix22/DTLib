namespace DTLib.ConsoleGUI
{
    public class Label : IDrawable
    {
        public (ushort x, ushort y) AnchorPoint { get; set; }
        public ushort Width { get; }
        public ushort Height { get; }
        public char[] Textmap { get; private set; }
        public char[] Colormap { get; private set; }

        public string TextmapFile { get; set; }
        public string ColormapFile { get; set; }

        public Label() { }

        public Label(string textmapFile, string colormapFile)
        {
            TextmapFile = textmapFile;
            ColormapFile = colormapFile;
        }

        public void GenColormap()
        {

        }

        public void GenTextmap()
        {

        }
    }
}
