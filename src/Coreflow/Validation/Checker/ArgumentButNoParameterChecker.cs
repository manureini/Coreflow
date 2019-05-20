using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coreflow.Interfaces;
using Coreflow.Validation.Messages;

namespace Coreflow.Validation.Checker
{
    internal class ArgumentButNoParameterChecker : ICodeCreatorChecker
    {
        public void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IParametrized para)
            {
                foreach (var arg in para.Arguments)
                {
                    if (!para.GetParameters().Any(p => p.Name == arg.Name))
                    {
                        AddToResult(ref pMessages, pCodeCreator, arg);
                    }
                }
            }
        }

        private void AddToResult(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator, IArgument pArgument)
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
    }
}
