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
            typeof(RemoveArgumentCorrector),
            typeof(MoveArgumentCorrector)
        };

        private static ICorrector CreateCorrector(Type pType, FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pMessages, IFlowValidationMessage pMessage)
        {
            return (ICorrector)Activator.CreateInstance(pType, new object[] { pFlowDefinition, pMessages, pMessage });
        }

        public static List<ICorrector> GetCorrectors(FlowDefinition pFlowDefinition, List<IFlowValidationMessage> pValidationMessages)
        {
            List<ICorrector> ret = new List<ICorrector>();

            foreach (var ctype in mCorrectors)
            {
                foreach (var msg in pValidationMessages)
                {
                    var corrector = CreateCorrector(ctype, pFlowDefinition, pValidationMessages, msg);

                    if (corrector.CanCorrect())
                        ret.Add(corrector);
                }
            }

            return ret;
        }
    }
}
