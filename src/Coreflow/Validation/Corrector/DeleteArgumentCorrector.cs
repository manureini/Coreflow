using Coreflow.Interfaces;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Corrector
{
    public class DeleteArgumentCorrector : AbstractCorrector
    {
        public override string Name => $"Delete {((ArgumentButNoParameterMessage)Message).Argument.Name} Argument";

        public DeleteArgumentCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage) : base(pFlowDefinition, pMessages, pMessage)
        {
        }

        public override bool CanCorrect()
        {
            return Message is ArgumentButNoParameterMessage;
        }

        public override object GetData()
        {
            return ((ArgumentButNoParameterMessage)Message).Argument.Name;
        }

        public static void Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)
        {
            string name = (string)pData;

            foreach (var ccid in pCodeCreators)
            {
                var cc = pFlowDefinition.FindCodeCreator(ccid) as IParametrized;
                cc.Arguments.RemoveAll(a => a.Name == name);
            }
        }
    }
}
