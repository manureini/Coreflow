using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public class ArgumentButNoParameterMessage : IFlowValidationCodeCreatorMessage
    {
        internal static void AddToResult(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator, IArgument pArgument)
        {
            string typeIdentifier = pCodeCreator.GetTypeIdentifier();

            ArgumentButNoParameterMessage msg = (ArgumentButNoParameterMessage)pMessages.FirstOrDefault(m => m is ArgumentButNoParameterMessage am && am.CodeCreatorTypeIdentifier == typeIdentifier && am.Argument.Name == pArgument.Name);

            if (msg == null)
            {
                pMessages.Add(new ArgumentButNoParameterMessage(typeIdentifier, pArgument, pCodeCreator.Identifier));
                return;
            }

            msg.CodeCreatorIdentifiers.Add(pCodeCreator.Identifier);
        }


        public FlowValidationMessageType MessageType => FlowValidationMessageType.ArgumentButNoParameter;

        public bool IsFatalError => false;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public IArgument Argument { get; }

        protected ArgumentButNoParameterMessage(string pCodeCreatorTypeIdentifier, IArgument pArgument, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Argument = pArgument;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
