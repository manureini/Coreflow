using System;

namespace Coreflow.Objects
{
    public class FlowCodeWriterWriteEventArgs : EventArgs
    {
        public string Code { get; set; }

        internal FlowCodeWriterWriteEventArgs() { }
    }
}
