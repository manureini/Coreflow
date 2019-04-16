using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Storage
{
    public class MemoryFlowDefinitionStorage : IFlowDefinitionStorage
    {
        private List<FlowDefinition> mFlowDefinitions = new List<FlowDefinition>();

        public void Add(FlowDefinition pFlowDefinition)
        {
            mFlowDefinitions.Add(pFlowDefinition);
        }

        public void Dispose()
        {
            mFlowDefinitions = null;
        }

        public IEnumerable<FlowDefinition> GetDefinitions()
        {
            return mFlowDefinitions;
        }

        public void Remove(Guid pIdentifier)
        {
            mFlowDefinitions.RemoveAll(w => w.Identifier == pIdentifier);
        }

        public void SetCoreflow(Coreflow pCoreflow)
        {
        }
    }
}
