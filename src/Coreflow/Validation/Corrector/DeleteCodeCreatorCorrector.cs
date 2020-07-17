using Coreflow.Interfaces;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;

namespace Coreflow.Validation.Corrector
{
    public class DeleteCodeCreatorCorrector : AbstractCorrector
    {
        public override string Name => $"Delete Code Creator";

        public DeleteCodeCreatorCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage) : base(pFlowDefinition, pMessages, pMessage)
        {
        }

        public override bool CanCorrect()
        {
            return Message is MissingCodeCreatorMessage;
        }

        public override object GetData()
        {
            return null;
        }

        public static void Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)
        {
            foreach (var ccid in pCodeCreators)
            {
                var cc = pFlowDefinition.FindParentCodeCreatorOf(ccid) as ICodeCreatorContainerCreator;

                if (cc == null)
                    return;

                foreach (var entry in cc.CodeCreators)
                {
                    entry.RemoveAll(c => c.Identifier == ccid);
                }
            }
        }
    }
}
