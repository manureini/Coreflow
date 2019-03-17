using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Interfaces
{
    public interface IWorkflowDefinitionStorage : IDisposable
    {
        void SetCoreflow(Coreflow pCoreflow); //Todo

        void Add(WorkflowDefinition pWorkflowDefinition);

        void Remove(Guid pIdentifier);

        IEnumerable<WorkflowDefinition> GetWorkflowDefinitions();
    }
}
