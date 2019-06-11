using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public interface IFlowValidationMessage : IIdentifiable
    {
        FlowValidationMessageType MessageType { get; }

        bool IsFatalError { get; }

        string Message { get; }
    }
}
