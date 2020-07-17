using System;

namespace Coreflow.Runtime
{
    public class SimpleFlowDefinition : IFlowDefinition
    {
        public string Name { get; set; }

        public Guid Identifier { get; set; }
    }
}
