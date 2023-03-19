using Coreflow.Interfaces;
using System.Collections.Generic;

namespace Coreflow.Helper
{
    public static class FlowDefinitionHelper
    {
        public static List<ICodeCreator> GetAllCodeCreators(FlowDefinition pFlowDefinition)
        {
            List<ICodeCreator> result = new List<ICodeCreator>();
            AddCodeCreator(pFlowDefinition.CodeCreator, ref result);
            return result;
        }

        private static void AddCodeCreator(ICodeCreator pCodeCreator, ref List<ICodeCreator> pResult)
        {
            pResult.Add(pCodeCreator);

            if (pCodeCreator is ICodeCreatorContainerCreator container)
            {
                if (container.CodeCreators != null)
                    foreach (var ccs in container.CodeCreators)
                    {
                        foreach (var cc in ccs)
                        {
                            AddCodeCreator(cc, ref pResult);
                        }
                    }
            }
        }
    }
}
