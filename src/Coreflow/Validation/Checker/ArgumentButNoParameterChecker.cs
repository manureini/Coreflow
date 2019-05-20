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
                        ArgumentButNoParameterMessage.AddToResult(ref pMessages, pCodeCreator, arg);
                    }
                }
            }
        }
    }
}
