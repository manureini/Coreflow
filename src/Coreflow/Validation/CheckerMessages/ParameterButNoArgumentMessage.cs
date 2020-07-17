using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Validation.Messages
{
    public class ParameterButNoArgumentMessage : IFlowValidationCodeCreatorMessage
    {
        public string Message => $"Parameter {Parameter.Name} is defined, but there is no argument with that name.";

        public bool IsFatalError => true;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public CodeCreatorParameter Parameter { get; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        internal ParameterButNoArgumentMessage(string pCodeCreatorTypeIdentifier, CodeCreatorParameter pParameter, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Parameter = pParameter;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
