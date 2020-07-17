using Coreflow.Validation.Messages;
using System.Collections.Generic;

namespace Coreflow.Validation
{
    public class FlowValidationResult
    {
        public bool IsValid { get; set; }

        public List<IFlowValidationMessage> Messages { get; internal set; } = new List<IFlowValidationMessage>();
    }
}
