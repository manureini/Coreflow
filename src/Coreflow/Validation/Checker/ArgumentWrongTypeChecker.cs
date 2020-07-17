using System.Collections.Generic;
using System.Linq;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Validation.CheckerMessages;
using Coreflow.Validation.Messages;

namespace Coreflow.Validation.Checker
{
    internal class ArgumentWrongTypeChecker : ICodeCreatorChecker
    {
        public void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is IParametrized para)
            {
                foreach (var param in para.GetParameters())
                {
                    var arg = para.Arguments.FirstOrDefault(a => a != null && a.Name == param.Name);

                    if (arg != null && arg is InputExpressionCreator iec && iec.Type != param.Type.AssemblyQualifiedName)
                    {
                        AddToResult(ref pMessages, pCodeCreator, param, arg);
                    }
                }
            }
        }

        private void AddToResult(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator, CodeCreatorParameter pParameter, IArgument pArgument)
        {
            string typeIdentifier = pCodeCreator.GetTypeIdentifier();

            var msg = (ArgumentWrongTypeMessage)pMessages.FirstOrDefault(m => m is ArgumentWrongTypeMessage am && am.CodeCreatorTypeIdentifier == typeIdentifier && am.Argument.Name == pArgument.Name);

            if (msg == null)
            {
                pMessages.Add(new ArgumentWrongTypeMessage(typeIdentifier, pParameter, pArgument, pCodeCreator.Identifier));
                return;
            }

            msg.CodeCreatorIdentifiers.Add(pCodeCreator.Identifier);
        }

    }
}
