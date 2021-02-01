using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Coreflow.Helper
{
    public class FlowManager : RuntimeFlowManager
    {
        private string mFullCode = "";

        public FlowManager(CoreflowRuntime pCoreflow) : base(pCoreflow)
        {
        }

        public FlowCompileResult CompileFlowsCreateAndLoadAssembly(IEnumerable<IFlowDefinition> pFlows, bool pDebug, bool pForceRecompile)
        {
            lock (mLocker)
            {
                var combinedCode = new StringBuilder();

                List<string> typenames = new List<string>();

                foreach (var flow in pFlows)
                {
                    FlowCode fcode = FlowBuilderHelper.GenerateFlowCode((FlowDefinition)flow);
                    combinedCode.Append(fcode.Code);
                    combinedCode.AppendLine();

                    typenames.Add($"typeof(global::{fcode.FullGeneratedTypeName})");
                }

                combinedCode.AppendLine();
                combinedCode.AppendLine();

                combinedCode.AppendLine("public static class GeneratedFlowsInfo { ");

                combinedCode.AppendLine("public static readonly global::System.Type[] FlowTypes = new global::System.Type[] {");

                if (typenames.Count > 0)
                {
                    combinedCode.Append(string.Join(",", typenames));
                }

                combinedCode.AppendLine(" };");

                combinedCode.AppendLine(" }");

                string fullcode = combinedCode.ToString();

                if (!pForceRecompile && fullcode == mFullCode)
                    return null;

                mFullCode = fullcode;

                foreach (var factory in mFactories)
                {
                    factory.Value.Dispose();
                }

                mFactories.Clear();

                var result = FlowCompilerHelper.CompileFlowCode(fullcode, pDebug, mCoreflow.TemporaryFilesDirectory);

                if (!result.Successful)
                    throw new Exception($"Flows did not compile! Message: {result.ErrorMessage}");

                /*   if (mAssemblyContext != null)
                   {
                       mAssemblyContext.Unload();

                       GC.Collect();
                       //GC.WaitForPendingFinalizers();
                   }

                   mAssemblyContext = new CollectibleAssemblyLoadContext();

                   Assembly asm = mAssemblyContext.LoadFromAssemblyPath(result.DllFilePath);*/

                Assembly asm;

                if (mCoreflow.TemporaryFilesDirectory == null)
                {
                    asm = AppDomain.CurrentDomain.Load(result.AssemblyBinary);
                }
                else
                {
                    asm = Assembly.LoadFile(result.DllFilePath);
                }

                UpdateFactories(asm);

                return result;
            }
        }
    }
}
