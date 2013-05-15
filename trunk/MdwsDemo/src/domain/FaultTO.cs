using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public class FaultTO
    {
        public string type;
        public string message;
        public string stackTrace;
        public string innerType;
        public string innerMessage;
        public string innerStackTrace;
        public string suggestion;

        public FaultTO()
        {
        }
    }
}
