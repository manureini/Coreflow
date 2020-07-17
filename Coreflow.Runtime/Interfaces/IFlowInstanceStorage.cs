using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Interfaces
{
    public interface IFlowInstanceStorage : IDisposable
    {
        void Add(FlowInstance pFlowInstance);

        void Update(FlowInstance pFlowInstance);

        IEnumerable<FlowInstance> GetInstances();
    }
}
