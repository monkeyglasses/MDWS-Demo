using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public class XRefTO : AbstractTO
    {
        public string dd;
        public string fieldName;
        public string fieldNumber;
        public VistaFileTO file;
        public string name;

        public XRefTO() { }
    }
}