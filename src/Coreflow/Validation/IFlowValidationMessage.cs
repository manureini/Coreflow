using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public interface IFlowValidationMessage
    {
        public FlowValidationMessageType MessageType { get; }

        public bool IsFatalError { get; }
    }
}
