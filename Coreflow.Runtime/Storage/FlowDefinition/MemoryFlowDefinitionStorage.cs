using Coreflow.Interfaces;
using Coreflow.Runtime;
using Coreflow.Runtime.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Storage
{
    public class MemoryFlowDefinitionStorage : IFlowDefinitionStorage
    {
        private List<IFlowDefinition> mFlowDefinitions = new List<IFlowDefinition>();

        public void Add(IFlowDefinition pFlowDefinition)
        {
            mFlowDefinitions.Add(pFlowDefinition);
        }

        public void Dispose()
        {
            mFlowDefinitions = null;
        }

        public IFlowDefinition Get(Guid pIdentifier)
        {
            return mFlowDefinitions.FirstOrDefault(f => f.Identifier == pIdentifier);
        }

        public IEnumerable<IFlowDefinition> GetDefinitions()
        {
            return mFlowDefinitions;
        }

        public void Remove(Guid pIdentifier)
        {
            mFlowDefinitions.RemoveAll(w => w.Identifier == pIdentifier);
        }

        public void SetCoreflow(CoreflowRuntime pCoreflow)
        {
        }
    }
}
