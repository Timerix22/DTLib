using DTLib.Dtsod;
using DTLib.Filesystem;
using System.Collections.Generic;
using static DTLib.PublicLog;

namespace DTLib.ConsoleGUI
{
    public class Container : List<IDrawable>, IDrawable
    {
        public (ushort x, ushort y) AnchorPoint { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public char[] Textmap { get; private set; }
        public char[] Colormap { get; private set; }
        public string Name { get; private set; }

        public Container(string name, string layout_file)
        {
            Name=name;
            ParseLayoutFile(layout_file);
        }

        void ParseLayoutFile(string layout_file)
        {
            DtsodV23 layout = new(File.ReadAllText(layout_file));
            AnchorPoint=(layout[Name]["anchor"][0], layout[Name]["anchor"][1]);
            Width=layout[Name]["width"];
            Height=layout[Name]["height"];
            foreach(string element_name in layout[Name]["children"].Keys)
            {
                switch(layout[Name]["children"][element_name]["type"])
                {
                    case "label":
                        Add(new Label(element_name,
                            layout[Name]["children"][element_name]["resdir"]+$"\\{element_name}.textmap",
                            layout[Name]["children"][element_name]["resdir"]+$"\\{element_name}.colormap")
                        {
                            AnchorPoint=(layout[Name]["children"][element_name]["anchor"][0],
                                    layout[Name]["children"][element_name]["anchor"][1])
                        });
                        break;
                }
            }
        }

        public void GenTextmap()
        {
            Textmap=new char[Width*Height];
            for(int i = 0; i<Textmap.Length; i++)
                Textmap[i]=' ';
            foreach(IDrawable element in this)
            {
                element.GenTextmap();
                Log("m", $"Length: {element.Textmap.Length} calculated: {element.Width*element.Height}\n");
                for(ushort y = 0; y<element.Height; y++)
                    for(ushort x = 0; x<element.Width; x++)
                    {
                        //Textmap[(element.AnchorPoint.y + y) * Width + element.AnchorPoint.x + x] = element.Textmap[y * element.Width + x];
                        Textmap[(y)*Width+x]=element.Textmap[y*element.Width+x];
                    }
            }
        }

        public void GenColormap()
        {

        }
    }
}
