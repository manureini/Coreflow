using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coreflow.Interfaces;
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
                        ParameterButNoArgumentMessage.AddToResult(ref pMessages, pCodeCreator, param);
                    }
                }
            }
        }
    }
}
