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
            MemoryFlowDefinitionStorage storage = new MemoryFlowDefinitionStorage();
            Coreflow ret = new Coreflow(storage);
                        
            ret.CodeCreatorStorage.AddCodeActivity(typeof(AdderActivity));

            return ret;
        }
    }
}
