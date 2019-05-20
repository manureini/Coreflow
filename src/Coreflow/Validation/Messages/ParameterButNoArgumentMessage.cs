using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public class ParameterButNoArgumentMessage : IFlowValidationCodeCreatorMessage
    {
        public FlowValidationMessageType MessageType => FlowValidationMessageType.ArgumentButNoParameter;

        public bool IsFatalError => false;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public CodeCreatorParameter Parameter { get; }

        internal ParameterButNoArgumentMessage(string pCodeCreatorTypeIdentifier, CodeCreatorParameter pParameter, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Parameter = pParameter;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
