using Coreflow.Runtime.Storage;

namespace Coreflow.Test.Tests
{
    internal static class TestHelper
    {
        internal static Coreflow InitCoreflow()
        {
            MemoryFlowDefinitionStorage dstorage = new MemoryFlowDefinitionStorage();
            MemoryFlowInstanceStorage istorage = new MemoryFlowInstanceStorage();

            Coreflow ret = new Coreflow(dstorage, istorage, null);

            ret.CodeCreatorStorage.AddCodeActivity(typeof(AdderActivity));

            return ret;
        }
    }
}
