using Coreflow.Interfaces;
using Coreflow.Validation.CheckerMessages;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Corrector
{
    public class ChangeArgumentTypeCorrector : AbstractCorrector
    {
        public override string Name => $"Change {((ArgumentWrongTypeMessage)Message).Argument.Name} Type";

        public ChangeArgumentTypeCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage) : base(pFlowDefinition, pMessages, pMessage)
        {
        }

        public override bool CanCorrect()
        {
            return Message is ArgumentWrongTypeMessage;
        }

        public override object GetData()
        {
            return ((ArgumentWrongTypeMessage)Message).Argument.Name;
        }

        public static void Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)
        {
            string name = (string)pData;

            foreach (var ccid in pCodeCreators)
            {
                var cc = pFlowDefinition.FindCodeCreator(ccid) as IParametrized;
                var iec = cc.Arguments.First(a => a.Name == name) as InputExpressionCreator;
                var parameter = cc.GetParameters().First(a => a.Name == name);

                iec.Type = parameter.Type.AssemblyQualifiedName;
            }
        }
    }
}
