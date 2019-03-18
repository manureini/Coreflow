using Coreflow.Helper;
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
        public void StartEmptyWorkflow()
        {
            WorkflowDefinition wfdef = mCoreflow.WorkflowDefinitionFactory.Create("empty");

            WorkflowInvoker.Invoke(wfdef);

            wfdef.ReferencedAssemblies.Clear();
            WorkflowInvoker.Invoke(wfdef);

            wfdef.ReferencedAssemblies = null;
            WorkflowInvoker.Invoke(wfdef);
        }

        [TestMethod]
        public void StartTestWorkflow1()
        {
            WorkflowDefinition wfdef = TestWorkflows.GetTestWorkflow1(mCoreflow);

            WorkflowCompileResult compileResult = wfdef.GenerateWorkflowCode().Compile();

            Type workflowtype = compileResult.ResultAssembly.GetTypes().First(t => typeof(ICompiledWorkflow).IsAssignableFrom(t));

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("START wf");

            stopwatch.Start();

            ICompiledWorkflow wf = Activator.CreateInstance(workflowtype) as ICompiledWorkflow;
            wf.Run();

            stopwatch.Stop();

            Console.WriteLine("Invoketime: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
        }
        
        [TestMethod]
        public void CheckResultTestWorkflow1()
        {
            WorkflowDefinition wfdef = TestWorkflows.GetTestWorkflow1(mCoreflow);

            WorkflowCompileResult compileResult = wfdef.GenerateWorkflowCode().Compile();

            Type workflowtype = compileResult.ResultAssembly.GetTypes().First(t => typeof(ICompiledWorkflow).IsAssignableFrom(t));

            ICompiledWorkflow wf = Activator.CreateInstance(workflowtype) as ICompiledWorkflow;
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
