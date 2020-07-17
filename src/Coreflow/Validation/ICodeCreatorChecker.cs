using Coreflow.Validation.Messages;
using System.Collections.Generic;

namespace Coreflow.Validation
{
    public interface ICodeCreatorChecker : IChecker
    {
        void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator);
    }
}
