using System;

namespace DTLib.Dtsod
{
    public class DtsodSerializableAttribute : Attribute
    {
        public DtsodVersion Version;
        public DtsodSerializableAttribute(DtsodVersion ver) => Version = ver;
    }
}
