using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Runtime;
using Coreflow.Runtime.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Coreflow.Helper
{
    public class FlowManager : RuntimeFlowManager
    {
        private string mFullCode = "";

        public FlowManager(CoreflowRuntime pCoreflow) : base(pCoreflow)
        {
        }

        public FlowCompileResult CompileFlowsCreateAndLoadAssembly(Coreflow pCoreflowInstance, IEnumerable<IFlowDefinition> pFlows)
        {
            lock (mLocker)
            {
                var combinedCode = new StringBuilder();

                List<string> typenames = new List<string>();

                foreach (var flow in pFlows)
                {
                    FlowCode fcode = FlowBuilderHelper.GenerateFlowCode((FlowDefinition)flow);
                    combinedCode.Append(fcode.Code);

                    typenames.Add($"typeof(global::{fcode.FullGeneratedTypeName})");
                }

                combinedCode.AppendLine("public static class GeneratedFlowsInfo { ");

                combinedCode.AppendLine("public static readonly global::System.Type[] FlowTypes = new global::System.Type[] {");

                if (typenames.Count > 0)
                {
                    combinedCode.Append(string.Join(",", typenames));
                }

                combinedCode.AppendLine(" };");

                combinedCode.AppendLine(" }");

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

                UpdateFactories(asm);

                return result;
            }
        }






    }
}
