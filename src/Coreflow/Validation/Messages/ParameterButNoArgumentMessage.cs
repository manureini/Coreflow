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
        internal static void AddToResult(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator, CodeCreatorParameter pParameter)
        {
            string typeIdentifier = pCodeCreator.GetTypeIdentifier();

            ParameterButNoArgumentMessage msg = (ParameterButNoArgumentMessage)pMessages.FirstOrDefault(m => m is ParameterButNoArgumentMessage am && am.CodeCreatorTypeIdentifier == typeIdentifier && am.Parameter.Name == pParameter.Name);

            if (msg == null)
            {
                pMessages.Add(new ParameterButNoArgumentMessage(typeIdentifier, pParameter, pCodeCreator.Identifier));
                return;
            }

            msg.CodeCreatorIdentifiers.Add(pCodeCreator.Identifier);
        }


        public FlowValidationMessageType MessageType => FlowValidationMessageType.ArgumentButNoParameter;

        public bool IsFatalError => false;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public CodeCreatorParameter Parameter { get; }

        protected ParameterButNoArgumentMessage(string pCodeCreatorTypeIdentifier, CodeCreatorParameter pParameter, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Parameter = pParameter;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
