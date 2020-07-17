using System;
using System.Collections.Generic;

namespace Coreflow.Runtime
{
    public interface IFlowDefinitionStorage : IDisposable
    {
        void SetCoreflow(Coreflow.Runtime.CoreflowRuntime pCoreflow); //Todo

        void Add(IFlowDefinition pFlowDefinition);

        void Remove(Guid pIdentifier);

        IFlowDefinition Get(Guid pIdentifier);

        IEnumerable<IFlowDefinition> GetDefinitions();
    }
}
