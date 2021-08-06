namespace DTLib.ConsoleGUI
{
    public interface IDrawable
    {
        public (ushort x, ushort y) AnchorPoint { get; set; }
        public ushort Width { get; }
        public ushort Height { get; }
        public char[] Textmap { get; }
        public char[] Colormap { get; }

        public void GenTextmap();

        public void GenColormap();

    }
}
