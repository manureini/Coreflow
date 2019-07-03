using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Corrector
{
    public class AddArgumentCorrector : AbstractCorrector
    {
        public override string Name => $"Add empty {((ParameterButNoArgumentMessage)Message).Parameter.Name} Argument";

        public AddArgumentCorrector(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage) : base(pFlowDefinition, pMessages, pMessage)
        {
        }

        public override bool CanCorrect()
        {
            return Message is ParameterButNoArgumentMessage;
        }

        public override object GetData()
        {
            return ((ParameterButNoArgumentMessage)Message).Parameter.Name;
        }

        public static void Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)
        {
            string name = (string)pData;

            foreach (var ccid in pCodeCreators)
            {
                var cc = pFlowDefinition.FindCodeCreator(ccid) as IParametrized;
                var parameter = cc.GetParameters().Single(p => p.Name == name);
                cc.Arguments.Add(ArgumentHelper.CreateArgument(parameter, name, parameter.Type.AssemblyQualifiedName, "", Guid.NewGuid()));
            }
        }
    }
}
