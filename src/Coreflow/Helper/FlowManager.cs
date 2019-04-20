using Coreflow.Interfaces;
using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Coreflow.Helper
{

    public class FlowManager
    {
        private const string ASSEMBLY_PREFIX = "FlowsAssembly_";

        internal int mAssemblyVersion = 0;

        internal Dictionary<Guid, FlowInstanceFactory> mFactories = new Dictionary<Guid, FlowInstanceFactory>();

        private CollectibleAssemblyLoadContext mAssemblyContext;

        private string mFullCode = "";

        public void CompileFlows(Coreflow pCoreflowInstance, IEnumerable<FlowDefinition> pFlows)
        {
            var combinedCode = new StringBuilder();

            List<MetadataReference> references = new List<MetadataReference>();

            //TODO add lock threadsafe

            foreach (var flow in pFlows)
            {
                FlowCode fcode = FlowBuilderHelper.GenerateFlowCode(flow);

                references.AddRange(fcode.ReferencedAssemblies);
                combinedCode.Append(fcode.Code);
            }

            string fullcode = combinedCode.ToString();

            if (fullcode == mFullCode)
                return;

            mFullCode = fullcode;


            foreach (var factory in mFactories)
            {
                factory.Value.Dispose();
            }

            mFactories.Clear();


            mAssemblyVersion++; //Interlocked?

            string assemblyName = ASSEMBLY_PREFIX + mAssemblyVersion;

            var result = FlowCompilerHelper.CompileFlowCode(fullcode, references.Distinct(), assemblyName);

            if (!result.Successful)
                throw new Exception("Flows did not compile!");

            if (mAssemblyContext != null)
            {
                mAssemblyContext.Unload();

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }

            mAssemblyContext = new CollectibleAssemblyLoadContext();

            result.ResultAssembly.Position = 0;
            Assembly asm = mAssemblyContext.LoadFromStream(result.ResultAssembly);
            result.ResultAssembly.Dispose();

            IEnumerable<Type> flows = asm.GetTypes().Where(t => typeof(ICompiledFlow).IsAssignableFrom(t));

            foreach (var flowtype in flows)
            {
                var attribute = flowtype.GetCustomAttribute<FlowIdentifierAttribute>();
                mFactories.Add(attribute.Identifier, new FlowInstanceFactory(pCoreflowInstance, attribute.Identifier, flowtype));
            }
        }

        public FlowInstanceFactory GetFactory(Guid pFlowIdentifier)
        {
            return mFactories[pFlowIdentifier];
        }
    }
}
