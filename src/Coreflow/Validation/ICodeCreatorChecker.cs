using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation
{
    public interface ICodeCreatorChecker : IChecker
    {
        void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator);
    }
}
