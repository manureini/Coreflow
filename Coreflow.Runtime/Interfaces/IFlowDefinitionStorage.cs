using System;
using System.Collections.Generic;

namespace Coreflow.Runtime
{
    public interface IFlowDefinitionStorage : IDisposable
    {
        void SetCoreflow(CoreflowRuntime pCoreflow);

        void Add(IFlowDefinition pFlowDefinition);

        void Remove(Guid pIdentifier);

        void Update(IFlowDefinition pFlowDefinition);

        IFlowDefinition Get(Guid pIdentifier);

        IEnumerable<IFlowDefinition> GetDefinitions();
    }
}
