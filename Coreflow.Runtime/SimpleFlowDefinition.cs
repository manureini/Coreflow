using Coreflow.Runtime.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Runtime
{
    public class SimpleFlowDefinition : IFlowDefinition
    {
        public string Name { get; set; }

        public Guid Identifier { get; set; }
    }
}
