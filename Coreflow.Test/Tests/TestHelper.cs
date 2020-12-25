using Coreflow.Runtime.Storage;

namespace Coreflow.Test.Tests
{
    internal static class TestHelper
    {
        internal static CoreflowService InitCoreflow()
        {
            MemoryFlowDefinitionStorage dstorage = new MemoryFlowDefinitionStorage();
            MemoryFlowInstanceStorage istorage = new MemoryFlowInstanceStorage();

            CoreflowService ret = new CoreflowService(dstorage, istorage, null);

            ret.CodeCreatorStorage.AddCodeActivity(typeof(AdderActivity));

            return ret;
        }
    }
}
