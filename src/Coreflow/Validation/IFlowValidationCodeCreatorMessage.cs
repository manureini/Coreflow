using System;
using System.Collections.Generic;

namespace Coreflow.Validation.Messages
{
    public interface IFlowValidationCodeCreatorMessage : IFlowValidationMessage
    {
        List<Guid> CodeCreatorIdentifiers { get; }

        string CodeCreatorTypeIdentifier { get; }
    }
}
