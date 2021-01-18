using Coreflow.Interfaces;
using Coreflow.Validation.Checker;
using Coreflow.Validation.Messages;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Validation
{
    public static class FlowValidationHelper
    {
        public static List<IChecker> Checkers = new List<IChecker>()
        {
            new ArgumentButNoParameterChecker(),
            new ParameterButNoArgumentChecker(),
            new WrongContainerCountChecker(),
            new ArgumentWrongTypeChecker(),
            new MissingCodeCreatorChecker()
        };

        public static FlowValidationResult Validate(FlowDefinition pFlowDefinition)
        {
            FlowValidationResult ret = new FlowValidationResult();

            List<IFlowValidationMessage> valResult = new List<IFlowValidationMessage>();
            InvokeCheckers(ref valResult, pFlowDefinition.CodeCreator);

            ret.IsValid = !valResult.Any(m => m.IsFatalError);
            ret.Messages = valResult;

            return ret;
        }

        private static void InvokeCheckers(ref List<IFlowValidationMessage> pMessages, ICodeCreator pCodeCreator)
        {
            foreach (var checker in Checkers)
            {
                if (checker is ICodeCreatorChecker cChecker)
                    cChecker.Check(ref pMessages, pCodeCreator);
            }

            if (pCodeCreator is ICodeCreatorContainerCreator container && container.CodeCreators != null)
            {
                foreach (var ccontainer in container.CodeCreators)
                    foreach (var cc in ccontainer)
                    {
                        InvokeCheckers(ref pMessages, cc);
                    }
            }
        }

    }
}
