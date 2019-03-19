using Coreflow.Interfaces;
using Coreflow.Web.Models;
using System;

namespace Coreflow.Web.Controllers
{
    public static class WorkflowDefinitionModelIdentifiableHelper
    {
        public static IIdentifiable FindIIdentifiable(WorkflowDefinitionModel pWorkflowDefinitionModel, Guid pGuid)
        {
            if (pWorkflowDefinitionModel.Identifier == pGuid)
                return pWorkflowDefinitionModel;

            return FindIIdentifiable(pWorkflowDefinitionModel.CodeCreatorModel, pGuid);
        }

        private static IIdentifiable FindIIdentifiable(CodeCreatorModel pCodeCreator, Guid pGuid)
        {
            if (pCodeCreator.Identifier == pGuid)
                return pCodeCreator;

            if (pCodeCreator.CodeCreatorModelsFirst != null)
                foreach (var codeCreatorModel in pCodeCreator.CodeCreatorModelsFirst)
                {
                    IIdentifiable found = FindIIdentifiable(codeCreatorModel, pGuid);
                    if (found != null)
                        return found;
                }

            return null;
        }
    }
}
