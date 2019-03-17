using Coreflow.Helper;
using Coreflow.Storage;
using Coreflow.Test.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Coreflow.Test
{
    [TestClass]
    public class SerializationTests
    {
        private Coreflow mCoreflow;

        [TestInitialize]
        public void Initialize()
        {
            mCoreflow = TestHelper.InitCoreflow();
        }

        [TestMethod]
        public void SerializeTestWorkflow1()
        {
            WorkflowDefinition wfdef = TestWorkflows.GetTestWorkflow1(mCoreflow);

            string xml = WorkflowDefinitionSerializer.Serialize(wfdef);

            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Length > 10);
        }


        [TestMethod]
        public void DeserializeTestWorkflow1()
        {
            WorkflowDefinition wfdef = TestWorkflows.GetTestWorkflow1(mCoreflow);

            string xml = WorkflowDefinitionSerializer.Serialize(wfdef);

            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Length > 10);

            WorkflowDefinition deserializedWfDef = WorkflowDefinitionSerializer.DeSerialize(xml, mCoreflow);

            Assert.IsNotNull(deserializedWfDef.Coreflow);
            Assert.AreEqual(wfdef.Identifier, deserializedWfDef.Identifier);
        }

        [TestCleanup]
        public void CleanUp()
        {
            mCoreflow.Dispose();
        }
    }
}
