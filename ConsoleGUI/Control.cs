namespace DTLib.ConsoleGUI
{
    public class Control : IDrawable
    {
        public (ushort x, ushort y) AnchorPoint { get; set; }
        public ushort Width { get; }
        public ushort Height { get; }
        public char[] Textmap { get; private set; }
        public char[] Colormap { get; private set; }

        public void GenColormap()
        {

        }

        public void GenTextmap()
        {

        }
    }
}
