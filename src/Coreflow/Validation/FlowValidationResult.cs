using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation
{
    public class FlowValidationResult
    {
        public bool IsValid { get; set; }

        public List<IFlowValidationMessage> Messages { get; internal set; } = new List<IFlowValidationMessage>();

    }
}
