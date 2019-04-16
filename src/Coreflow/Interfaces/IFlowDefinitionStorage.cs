﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Interfaces
{
    public interface IFlowDefinitionStorage : IDisposable
    {
        void SetCoreflow(Coreflow pCoreflow); //Todo

        void Add(FlowDefinition pFlowDefinition);

        void Remove(Guid pIdentifier);

        IEnumerable<FlowDefinition> GetFlowDefinitions();
    }
}