using System;

namespace MdwsDemo.domain
{
    [Serializable]
    public abstract class AbstractTO
    {
        public FaultTO fault;

        public AbstractTO() { }
    }
}
