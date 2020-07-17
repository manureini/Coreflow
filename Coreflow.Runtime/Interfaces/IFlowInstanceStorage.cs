using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Runtime
{
    public interface IFlowInstanceStorage : IDisposable
    {
        void Add(FlowInstance pFlowInstance);

        void Update(FlowInstance pFlowInstance);

        IEnumerable<FlowInstance> GetInstances();
    }
}
