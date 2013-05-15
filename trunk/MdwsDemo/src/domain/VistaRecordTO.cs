using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public class VistaRecordTO : AbstractTO
    {
        public VistaFileTO file;
        public VistaFieldTO[] fields;
        public String ien;
        public String iens;
        public String siteId;

        public VistaRecordTO() { }
    }
}