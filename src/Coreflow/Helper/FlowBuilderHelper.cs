using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Runtime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coreflow
{
    internal class FlowBuilderHelper
    {
        internal const string FLOW_NAMESPACE_PREFIX = "FlowNs_";
        internal const string FLOW_CLASS_PREFIX = "Flow_";


        public static IEnumerable<FlowCode> GenerateFlowCode(IEnumerable<FlowDefinition> pFlows)
        {
            return pFlows.Select(w => GenerateFlowCode(w));
        }

        private static void SetParentContainer(ICodeCreator pCodeCreator, ICodeCreatorContainerCreator pParent = null)
        {
            if (pCodeCreator is ICodeCreatorContainerCreator container)
            {
                container.ParentContainerCreator = pParent;

                foreach (var entry in container.CodeCreators)
                {
                    foreach (var cc in entry)
                    {
                        SetParentContainer(cc, container);
                    }
                }
            }
        }

        public static FlowCode GenerateFlowCode(FlowDefinition pFlowDefinition, bool pOnlySingleFlow = false)
        {
            FlowCode ret = new FlowCode();
            ret.Definition = pFlowDefinition;

            SetParentContainer(pFlowDefinition.CodeCreator);

            FlowCodeWriter cw = new FlowCodeWriter();

            FlowBuilderContext context = new FlowBuilderContext(cw, pFlowDefinition);

            string flowid = pFlowDefinition.Identifier.ToString().ToVariableName();

            ret.FullGeneratedTypeName = FLOW_NAMESPACE_PREFIX + flowid + "." + FLOW_CLASS_PREFIX + flowid;

            //   cw.AppendLineTop("[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(\".NETCoreApp, Version = v3.0\", FrameworkDisplayName = \"\")]");

            cw.AppendLineTop("namespace " + FLOW_NAMESPACE_PREFIX + flowid + " {");

            cw.AppendLineBottom("} /* Namespace */"); //Close Namespace

            foreach (string import in pFlowDefinition.ReferencedNamespaces.Distinct())
            {
                cw.AppendLineTop($"using {import};");
            }

            cw.AppendLineTop();

            cw.WriteIdentifierTagTop(pFlowDefinition);
            cw.WriteContainerTagTop(pFlowDefinition);

            cw.AppendLineTop($"[global::{typeof(FlowIdentifierAttribute).FullName}(\"{pFlowDefinition.Identifier}\",\"{pFlowDefinition.Name}\")]");

            //Currently idk which letters needs an escape
            cw.AppendLineTop("public class " + FLOW_CLASS_PREFIX + flowid + " : " + typeof(ICompiledFlow).FullName + "  {");


            cw.AppendLineTop();

            cw.AppendLineBottom("} /* Class */"); //Close Class


            cw.AppendLineTop($"public global::System.Guid " + nameof(ICompiledFlow.InstanceId) + " {get; set;} = Guid.NewGuid();");

            cw.AppendLineTop($"public global::{typeof(CoreflowRuntime).FullName} {nameof(ICompiledFlow.CoreflowInstace)}");
            cw.AppendTop("{ get; set; }");

            cw.AppendLineTop($"public global::{typeof(IArgumentInjectionStore).FullName} {nameof(ICompiledFlow.ArgumentInjectionStore)}");
            cw.AppendTop("{ get; set; }");

            cw.AppendLineTop($"public global::{typeof(ILogger).FullName} {nameof(ICompiledFlow.Logger)}");
            cw.AppendTop("{ get; set; }");


            cw.AppendLineTop();

            if (pFlowDefinition.Arguments != null)
                foreach (FlowArgument arg in pFlowDefinition.Arguments)
                {
                    if (arg.Type == null)
                    {
                        cw.AppendLineTop($"//ERROR Parameter {arg.Name}: Type not found");
                        continue;
                    }

                    var typeCode = TypeHelper.TypeNameToCode(arg.Type);

                    string value = $"default({typeCode})";

                    if (arg.Expression != null && !string.IsNullOrWhiteSpace(arg.Expression))
                        value = arg.Expression;

                    cw.AppendLineTop($"public {typeCode} {arg.Name} = {value};");
                }


            cw.AppendLineTop();


            cw.AppendLineTop("public void SetArguments(" + TypeHelper.TypeNameToCode(typeof(IDictionary<string, object>)) + " pArguments) {");

            if (pFlowDefinition.Arguments != null)
                foreach (FlowArgument arg in pFlowDefinition.Arguments)
                {
                    if (arg.Type == null)
                        continue;
                    cw.AppendLineTop("if (pArguments.ContainsKey(\"" + arg.Name + "\"))  { " + arg.Name + " = (" + TypeHelper.TypeNameToCode(arg.Type) + ")pArguments[\"" + arg.Name + "\"];  }");
                }

            cw.AppendLineTop("}");

            cw.AppendLineTop();


            cw.AppendLineTop("public " + TypeHelper.TypeNameToCode(typeof(IDictionary<string, object>)) + " GetArguments() {");
            cw.AppendLineTop("var ret = new " + TypeHelper.TypeNameToCode(typeof(Dictionary<string, object>)) + "()");

            if (pFlowDefinition.Arguments != null && pFlowDefinition.Arguments.Any())
            {
                cw.AppendLineTop("{");
                cw.AppendLineTop();

                for (int i = 0; i < pFlowDefinition.Arguments.Count; i++)
                {
                    FlowArgument arg = pFlowDefinition.Arguments[i];
                    if (arg.Type == null)
                        continue;

                    cw.AppendTop("{\"" + arg.Name + "\", " + arg.Name + "}");

                    if (i < pFlowDefinition.Arguments.Count - 1)
                    {
                        cw.AppendTop(",");
                    }
                }

                cw.AppendLineTop("}");
            }

            cw.AppendTop(";");

            cw.AppendLineTop("return ret;");
            cw.AppendLineTop("}");

            cw.AppendLineTop();

            cw.AppendLineTop();

            cw.AppendLineTop("public void Run() { ");

            cw.AppendLineBottom("} /* Run */"); //Close Run


            if (pFlowDefinition.CodeCreator != null)
            {
                if (pFlowDefinition.CodeCreator is IVariableCreator ccv)
                    ccv.Initialize(context, cw);

                pFlowDefinition.CodeCreator.ToCode(context, cw);
            }

            ret.Code = cw.ToString();
            return ret;
        }

        public static IVariableCreator GetVariableCreatorInInitialScope(ICodeCreatorContainerCreator pContainer, Func<IVariableCreator, bool> pFilter)
        {
            if (pContainer == null)
                return null;

            foreach (var cclist in pContainer.CodeCreators)
            {
                IVariableCreator found = cclist.Select(v => v as IVariableCreator).Where(v => v != null).FirstOrDefault(pFilter);
                if (found != null)
                    return found;
            }

            return GetVariableCreatorInInitialScope(pContainer.ParentContainerCreator, pFilter);
        }

        public static string FormatCode(string pCode)
        {
            return FormatCode(FlowCompilerHelper.ParseTextNotDebuggable(pCode));
        }

        private static string FormatCode(SyntaxTree pSyntaxTree)
        {
            var workspace = new AdhocWorkspace();

            SyntaxNode formattedNode = Formatter.Format(pSyntaxTree.GetRoot(), workspace);

            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                formattedNode.WriteTo(writer);
            }

            workspace.Dispose();
            return sb.ToString();
        }
    }
}
