using Coreflow.Helper;
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
        public void SerializeTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);

            string xml = FlowDefinitionSerializer.Serialize(wfdef);

            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Length > 10);
        }


        [TestMethod]
        public void DeserializeTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);

            string xml = FlowDefinitionSerializer.Serialize(wfdef);

            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Length > 10);

            FlowDefinition deserializedWfDef = FlowDefinitionSerializer.Deserialize(xml, mCoreflow);

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
