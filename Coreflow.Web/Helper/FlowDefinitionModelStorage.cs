using Coreflow.Web.Controllers;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Helper
{
    public static class FlowDefinitionModelStorage
    {
        private static Dictionary<Guid, FlowDefinitionModel> mModels = new Dictionary<Guid, FlowDefinitionModel>();

        public static void StoreModel(FlowDefinitionModel pModel, bool pPersistent)
        {
            mModels.Remove(pModel.Identifier);
            mModels.Add(pModel.Identifier, pModel);

            if (pPersistent)
                PersistModel(pModel.Identifier);
        }

        public static void PersistModel(Guid pIdentifier)
        {
            if (!mModels.ContainsKey(pIdentifier)) //not loaded yet
                return;

            FlowDefinition fdef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(mModels[pIdentifier]);
            fdef.Coreflow = Program.CoreflowInstance;

            Program.CoreflowInstance.FlowDefinitionStorage.Remove(fdef.Identifier);
            Program.CoreflowInstance.FlowDefinitionStorage.Add(fdef);
        }

        public static FlowDefinitionModel GetModel(Guid pIdentifier)
        {
            if (mModels.ContainsKey(pIdentifier))
                return mModels[pIdentifier];

            var fdef = Program.CoreflowInstance.FlowDefinitionStorage.GetDefinitions().FirstOrDefault(d => d.Identifier == pIdentifier);
            var fmodel = FlowDefinitionModelMappingHelper.GenerateModel(fdef);

            mModels.Add(fmodel.Identifier, fmodel);
            return fmodel;
        }
    }
}
