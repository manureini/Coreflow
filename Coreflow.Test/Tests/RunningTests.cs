using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Storage;
using Coreflow.Test.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

            FlowInvoker.Invoke(wfdef);

            wfdef.ReferencedAssemblies.Clear();
            FlowInvoker.Invoke(wfdef);

            wfdef.ReferencedAssemblies = null;
            FlowInvoker.Invoke(wfdef);
        }


        [TestMethod]
        public void CreateCodeTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);

            FlowCompileResult compileResult = wfdef.GenerateFlowCode().Compile();

            Console.WriteLine(compileResult.FlowCode.Code);
        }


        [TestMethod]
        public void StartTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);

            FlowCompileResult compileResult = wfdef.GenerateFlowCode().Compile();

            Type Flowtype = compileResult.ResultAssembly.GetTypes().First(t => typeof(ICompiledFlow).IsAssignableFrom(t));

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("START wf");

            stopwatch.Start();

            ICompiledFlow wf = Activator.CreateInstance(Flowtype) as ICompiledFlow;
            wf.Run();

            stopwatch.Stop();

            Console.WriteLine("Invoketime: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
        }

        [TestMethod]
        public void CheckResultTestFlow1()
        {
            FlowDefinition wfdef = TestFlows.GetTestFlow1(mCoreflow);

            FlowCompileResult compileResult = wfdef.GenerateFlowCode().Compile();

            Type Flowtype = compileResult.ResultAssembly.GetTypes().First(t => typeof(ICompiledFlow).IsAssignableFrom(t));

            ICompiledFlow wf = Activator.CreateInstance(Flowtype) as ICompiledFlow;
            wf.Run();

            int result = (int)wf.GetType().GetField("Result").GetValue(wf);

            Assert.AreEqual(3, result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            mCoreflow.Dispose();
        }
    }
}
