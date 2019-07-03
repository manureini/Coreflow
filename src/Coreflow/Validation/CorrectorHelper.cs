using Coreflow.Validation.Corrector;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation
{
    public static class CorrectorHelper
    {
        private static List<Type> mCorrectors = new List<Type>()
        {
            typeof(AddArgumentCorrector),
            typeof(DeleteArgumentCorrector),
            typeof(MoveArgumentCorrector),
            typeof(ChangeArgumentTypeCorrector),
            typeof(DeleteCodeCreatorCorrector)
        };

        private static ICorrector CreateCorrector(Type pType, FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage)
        {
            return (ICorrector)Activator.CreateInstance(pType, new object[] { pFlowDefinition, pMessages, pMessage });
        }

        public static List<(Guid, ICorrector)> GetCorrectors(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pValidationMessages)
        {
            List<(Guid, ICorrector)> ret = new List<(Guid, ICorrector)>();

            foreach (var ctype in mCorrectors)
            {
                foreach (var msg in pValidationMessages)
                {
                    var corrector = CreateCorrector(ctype, pFlowDefinition, pValidationMessages, msg);

                    if (corrector.CanCorrect())
                        ret.Add((msg.Identifier, corrector));
                }
            }

            return ret;
        }
    }
}
