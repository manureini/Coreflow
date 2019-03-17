using System;

namespace Coreflow.Objects
{
    public class WorkflowCodeWriterWriteEventArgs : EventArgs
    {
        public string Code { get; set; }

        internal WorkflowCodeWriterWriteEventArgs() { }
    }
}
