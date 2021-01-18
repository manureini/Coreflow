using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Validation.Messages;

namespace Coreflow.Validation.Checker
{
    internal class WrongContainerCountChecker : ICodeCreatorChecker
    {
        public void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is ICodeCreatorContainerCreator container && container.CodeCreators != null)
            {
                if (container.SequenceCount != container.CodeCreators.Count)
                {
                    pMessages.Add(new WrongContainerCountMessage(container.Identifier, container.CodeCreators.Count, container.SequenceCount));
                }
            }
        }
    }
}
