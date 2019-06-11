using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Coreflow.Helper
{
    public class FlowManager
    {
        internal Dictionary<Guid, FlowInstanceFactory> mFactories = new Dictionary<Guid, FlowInstanceFactory>();

        //  private AssemblyCon mAssemblyContext;

        private string mFullCode = "";

        private object mLocker = new object();

        public FlowCompileResult CompileFlowsCreateAndLoadAssembly(Coreflow pCoreflowInstance, IEnumerable<FlowDefinition> pFlows)
        {
            lock (mLocker)
            {
                var combinedCode = new StringBuilder();

                foreach (var flow in pFlows)
                {
                    FlowCode fcode = FlowBuilderHelper.GenerateFlowCode(flow);
                    combinedCode.Append(fcode.Code);
                }

                string fullcode = combinedCode.ToString();

                if (fullcode == mFullCode)
                    return null;

                mFullCode = fullcode;

                foreach (var factory in mFactories)
                {
                    factory.Value.Dispose();
                }

                mFactories.Clear();


                string assemblyName = "Flows";

                var result = FlowCompilerHelper.CompileFlowCode(fullcode, true, assemblyName);

                if (!result.Successful)
                    throw new Exception("Flows did not compile!");

                /*   if (mAssemblyContext != null)
                   {
                       mAssemblyContext.Unload();

                       GC.Collect();
                       //GC.WaitForPendingFinalizers();
                   }

                   mAssemblyContext = new CollectibleAssemblyLoadContext();

                   Assembly asm = mAssemblyContext.LoadFromAssemblyPath(result.DllFilePath);*/

                Assembly asm = Assembly.LoadFile(result.DllFilePath);


                IEnumerable<Type> flows = asm.GetTypes().Where(t => typeof(ICompiledFlow).IsAssignableFrom(t));

                foreach (var flowtype in flows)
                {
                    var attribute = flowtype.GetCustomAttribute<FlowIdentifierAttribute>();
                    mFactories.Add(attribute.Identifier, new FlowInstanceFactory(pCoreflowInstance, attribute.Identifier, flowtype));
                }

                return result;
            }
        }

        public FlowInstanceFactory GetFactory(Guid pFlowIdentifier)
        {
            lock (mLocker)
            {
                return mFactories[pFlowIdentifier];
            }
        }
    }
}
