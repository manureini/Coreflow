using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public interface IFlowValidationMessage : IIdentifiable
    {
        public FlowValidationMessageType MessageType { get; }

        public bool IsFatalError { get; }

        public string Message { get; }
    }
}
