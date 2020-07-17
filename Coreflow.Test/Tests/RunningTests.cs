using Coreflow.Test.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Coreflow.Test
{
    [TestClass]
    public class RunningTests
    {
        private Coreflow mCoreflow;

        [TestInitialize]
        public void Initialize()
        {
            mCoreflow = TestHelper.InitCoreflow();
        }


        [TestMethod]
        public void StartEmptyFlow()
        {
            FlowDefinition wfdef = mCoreflow.FlowDefinitionFactory.Create("empty");

        }


        [TestMethod]
        public void CreateCodeTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);
            mCoreflow.FlowDefinitionStorage.Add(wfdef);
            mCoreflow.CompileFlows();
            mCoreflow.RunFlow(wfdef.Identifier);
        }

        [TestMethod]
        public void StartTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);
            mCoreflow.FlowDefinitionStorage.Add(wfdef);
            mCoreflow.CompileFlows();
            mCoreflow.RunFlow(wfdef.Identifier);
        }

        [TestCleanup]
        public void CleanUp()
        {
            mCoreflow.Dispose();
        }
    }
}
