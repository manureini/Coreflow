using Coreflow.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Test.Tests
{
    internal static class TestHelper
    {
        internal static Coreflow InitCoreflow()
        {
            MemoryFlowDefinitionStorage dstorage = new MemoryFlowDefinitionStorage();
            MemoryFlowInstanceStorage istorage = new MemoryFlowInstanceStorage();

            Coreflow ret = new Coreflow(dstorage, istorage);

            ret.CodeCreatorStorage.AddCodeActivity(typeof(AdderActivity));

            return ret;
        }
    }
}
