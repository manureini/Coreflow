using Coreflow.Storage;
using Coreflow.Storage.ArgumentInjection;
using Coreflow.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Test.Tests
{
    [TestClass]
    public class ValidationTest
    {
        private Coreflow mCoreflow;

        [TestInitialize]
        public void Initialize()
        {
            mCoreflow = new Coreflow(
               new SimpleFlowDefinitionFileStorage(@"C:\GitHub\Coreflow\Coreflow.Repository\Flows"),
               new SimpleFlowInstanceFileStorage("FlowInstances"),
               new JsonFileArgumentInjectionStore("Arguments.json")
              );
        }


        [TestMethod]
        public void ValidateFlows()
        {
            foreach (var definition in mCoreflow.FlowDefinitionStorage.GetDefinitions())
            {
                var result = FlowValidationHelper.Validate(definition);


            }
        }

    }
}
