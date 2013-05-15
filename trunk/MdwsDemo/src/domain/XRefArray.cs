using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public class XRefArray : AbstractArrayTO
    {
        public XRefTO[] xrefs;

        public XRefArray() { }
    }
}