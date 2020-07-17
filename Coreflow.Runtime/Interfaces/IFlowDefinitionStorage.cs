using Coreflow.Runtime.Interfaces;
using System;
using System.Collections.Generic;

namespace Coreflow.Interfaces
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
