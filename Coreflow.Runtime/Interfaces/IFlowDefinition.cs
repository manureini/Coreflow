using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Runtime.Interfaces
{
    public interface IFlowDefinition : IIdentifiable
    {
        string Name { get; set; }
    }
}
