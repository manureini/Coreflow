using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Storage
{
    public class MemoryFlowInstanceStorage : IFlowInstanceStorage
    {
        private List<FlowInstance> mInstances = new List<FlowInstance>();

        public void Add(FlowInstance pFlowInstance)
        {
            mInstances.Add(pFlowInstance);
        }

        public void Update(FlowInstance pFlowInstance)
        {

        }

        public void Dispose()
        {
            mInstances.Clear();
        }

        public IEnumerable<FlowInstance> GetInstances()
        {
            return mInstances;
        }

    }
}
