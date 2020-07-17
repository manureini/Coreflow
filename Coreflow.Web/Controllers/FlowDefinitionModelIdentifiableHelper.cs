using Coreflow.Runtime;
using Coreflow.Web.Models;
using System;

namespace Coreflow.Web.Controllers
{
    public static class FlowDefinitionModelIdentifiableHelper
    {
        public static IIdentifiable FindIIdentifiable(FlowDefinitionModel pFlowDefinitionModel, Guid pGuid)
        {
            if (pFlowDefinitionModel.Identifier == pGuid)
                return pFlowDefinitionModel;

            return FindIIdentifiable(pFlowDefinitionModel.CodeCreatorModel, pGuid);
        }

        private static IIdentifiable FindIIdentifiable(CodeCreatorModel pCodeCreator, Guid pGuid)
        {
            if (pCodeCreator.Identifier == pGuid)
                return pCodeCreator;

            if (pCodeCreator.CodeCreatorModels != null)
                foreach (var codeCreatorModels in pCodeCreator.CodeCreatorModels)
                {
                    foreach (var codeCreatorModel in codeCreatorModels.Value)
                    {
                        IIdentifiable found = FindIIdentifiable(codeCreatorModel, pGuid);
                        if (found != null)
                            return found;
                    }
                }

            return null;
        }
    }
}
