using System.Collections.Generic;
using System.Linq;
using Coreflow.CodeCreators;
using Coreflow.Validation.Messages;

namespace Coreflow.Validation.Checker
{
    internal class MissingCodeCreatorChecker : ICodeCreatorChecker
    {
        public void Check(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is MissingCodeCreatorCreator mcc)
            {
                AddToResult(ref pMessages, pCodeCreator, mcc);
            }
        }

        private void AddToResult(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator, MissingCodeCreatorCreator pMissingcodeCreator)
        {
            MissingCodeCreatorMessage msg = (MissingCodeCreatorMessage)pMessages.FirstOrDefault(m => m is MissingCodeCreatorMessage mi && mi.Type == pMissingcodeCreator.Type && mi.FactoryIdentifier == pMissingcodeCreator.FactoryIdentifier);

            if (msg == null)
            {
                pMessages.Add(new MissingCodeCreatorMessage(pMissingcodeCreator.Type, pMissingcodeCreator.FactoryIdentifier, pCodeCreator.Identifier));
                return;
            }

            //TODO ?
        }

    }
}
