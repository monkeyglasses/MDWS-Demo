using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public class VistaFieldTO : AbstractTO
    {
        public bool isMultiple;
        public bool isPointer;
        public bool isWordProc;
        public string name;
        public string nodePiece;
        public string number;
        public VistaFileTO pointsTo;
        public string transform;
        public string type;
        public string value;

        public VistaFieldTO() { }
    }
}