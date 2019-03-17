using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Storage
{
    public class MemoryWorkflowDefinitionStorage : IWorkflowDefinitionStorage
    {
        private List<WorkflowDefinition> mWorkflowDefinitions = new List<WorkflowDefinition>();

        public void Add(WorkflowDefinition pWorkflowDefinition)
        {
            mWorkflowDefinitions.Add(pWorkflowDefinition);
        }

        public void Dispose()
        {
            mWorkflowDefinitions = null;
        }

        public IEnumerable<WorkflowDefinition> GetWorkflowDefinitions()
        {
            return mWorkflowDefinitions;
        }

        public void Remove(Guid pIdentifier)
        {
            mWorkflowDefinitions.RemoveAll(w => w.Identifier == pIdentifier);
        }

        public void SetCoreflow(Coreflow pCoreflow)
        {
        }
    }
}
