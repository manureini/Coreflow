using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public interface IFlowValidationCodeCreatorMessage : IFlowValidationMessage
    {
        List<Guid> CodeCreatorIdentifiers { get; }

        string CodeCreatorTypeIdentifier { get; }
    }
}
