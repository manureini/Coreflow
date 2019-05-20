using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Validation.Messages;

namespace Coreflow.Validation.Checker
{
    internal class ParameterButNoArgumentChecker : ICodeCreatorChecker
    {
        public void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IParametrized para)
            {
                foreach (var param in para.GetParameters())
                {
                    if (!para.Arguments.Any(a => a.Name == param.Name))
                    {
                        AddToResult(ref pMessages, pCodeCreator, param);
                    }
                }
            }
        }

        private void AddToResult(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator, CodeCreatorParameter pParameter)
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

    }
}
