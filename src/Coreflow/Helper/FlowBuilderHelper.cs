using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coreflow
{
    internal class FlowBuilderHelper
    {
        private const string INSTANCE_ID_PARAMETER_NAME = "InstanceId";

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

        public static FlowCode GenerateFlowCode(FlowDefinition pFlowDefinition)
        {
            FlowCode ret = new FlowCode();
            ret.Definition = pFlowDefinition;

            SetParentContainer(pFlowDefinition.CodeCreator);

            var wfReferencesDict = ReferenceHelper.GetMetadataReferences(pFlowDefinition);

            ret.ReferencedAssemblies = wfReferencesDict.Values;


            FlowCodeWriter cw = new FlowCodeWriter();

            FlowBuilderContext context = new FlowBuilderContext(cw, ret.ReferencedAssemblies.ToList());


            cw.AppendLineTop("namespace DynamicGeneratedFlow" + pFlowDefinition.Identifier.ToString().ToVariableName() + " {");

            cw.AppendLineBottom("} /* Namespace */"); //Close Namespace

            foreach (string import in pFlowDefinition.ReferencedNamespaces.Distinct())
            {
                cw.AppendLineTop($"using {import};");
            }

            cw.AppendLineTop();

            cw.WriteIdentifierTagTop(pFlowDefinition);
            cw.WriteContainerTagTop(pFlowDefinition);

            //Currently idk which letters needs an escape
            cw.AppendLineTop("public class wf_" + pFlowDefinition.Name.Replace(" ", "") + " : " + typeof(ICompiledFlow).FullName + "  {");



            cw.AppendLineTop();

            cw.AppendLineBottom("} /* Class */"); //Close Class


            cw.AppendLineTop($"public Guid {INSTANCE_ID_PARAMETER_NAME} = Guid.NewGuid();");

            cw.AppendLineTop();

            if (pFlowDefinition.Arguments != null)
                foreach (FlowArguments parameter in pFlowDefinition.Arguments)
                {
                    if (!wfReferencesDict.ContainsKey(parameter.Type.Assembly))
                        throw new ArgumentException($"Flow has parameter with type {parameter.Type}, but does not reference assembly {parameter.Type.Assembly.FullName}");

                    string value = $"default({parameter.Type.FullName})";

                    if (parameter.Expression != null && !string.IsNullOrWhiteSpace(parameter.Expression))
                        value = parameter.Expression;

                    cw.AppendLineTop($"public {parameter.Type.FullName} {parameter.Name} = {value};");
                }



            cw.AppendLineTop();


            cw.AppendLineTop("public void SetArguments(IDictionary<string, object> pArguments) {");

            if (pFlowDefinition.Arguments != null)
                foreach (FlowArguments arg in pFlowDefinition.Arguments)
                {
                    cw.AppendLineTop("if (pArguments.ContainsKey(\"" + arg.Name + "\"))  { " + arg.Name + " = (" + arg.Type.FullName + ")pArguments[\"" + arg.Name + "\"];  }");
                }

            cw.AppendLineTop("}");

            cw.AppendLineTop();


            cw.AppendLineTop("public IDictionary<string, object> GetArguments() {");
            cw.AppendLineTop("Dictionary<string, object> ret = new Dictionary<string, object>();");

            if (pFlowDefinition.Arguments != null)
                foreach (FlowArguments arg in pFlowDefinition.Arguments)
                {
                    cw.AppendLineTop("ret.Add(\"" + arg.Name + "\", " + arg.Name + ");");
                }

            cw.AppendLineTop("return ret;");
            cw.AppendLineTop("}");

            cw.AppendLineTop();

            cw.AppendLineTop("Guid Coreflow.Interfaces.ICompiledFlow.InstanceId => InstanceId;");

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

        public static IVariableCreator GetVariableCreatorInInitialScope(ICodeCreatorContainerCreator pContainer, ICodeCreator pCodeCreator, Func<IVariableCreator, bool> pFilter)
        {
            if (pContainer == null)
                return null;

            foreach (var cclist in pContainer.CodeCreators)
            {
                IVariableCreator found = cclist.Select(v => v as IVariableCreator).Where(v => v != null).FirstOrDefault(pFilter);
                if (found != null)
                    return found;
            }

            return GetVariableCreatorInInitialScope(pContainer.ParentContainerCreator, pContainer, pFilter);
        }

        public static string FormatCode(string pCode)
        {
            return FormatCode(FlowCompilerHelper.ParseText(pCode));
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

            return sb.ToString();
        }
    }
}
