using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public class ArgumentButNoParameterMessage : IFlowValidationCodeCreatorMessage
    {
        public FlowValidationMessageType MessageType => FlowValidationMessageType.ArgumentButNoParameter;

        public bool IsFatalError => false;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public IArgument Argument { get; }

        internal ArgumentButNoParameterMessage(string pCodeCreatorTypeIdentifier, IArgument pArgument, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Argument = pArgument;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
