using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public class VistaFileTO : AbstractTO
    {
        public VistaFieldTO[] fields;
        public string global;
        public string lastIenAssigned;
        public string name;
        public string number;
        public string numberOfRecords;
        public XRefArray xrefs;

        public VistaFileTO() { }
    }
}