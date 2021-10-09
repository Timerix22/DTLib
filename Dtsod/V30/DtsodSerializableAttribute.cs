using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTLib.Dtsod
{
    public class DtsodSerializableAttribute : Attribute
    {
        public DtsodVersion Version;
        public DtsodSerializableAttribute(DtsodVersion ver) => Version=ver;
    }
}
