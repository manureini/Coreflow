using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Runtime.Storage
{
    public class NoFlowInstanceStorage : IFlowInstanceStorage
    {
        public void Add(FlowInstance pFlowInstance)
        {
        }

        public void Dispose()
        {
        }

        public IEnumerable<FlowInstance> GetInstances()
        {
            throw new NotImplementedException();
        }

        public void Update(FlowInstance pFlowInstance)
        {
        }
    }
}
