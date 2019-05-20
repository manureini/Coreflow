using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public enum FlowValidationMessageType
    {
        ArgumentButNoParameter,
        ParameterButNoArgument,
        WrongCodeCreatorContainerCount
    }
}
