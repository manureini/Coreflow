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

        public static FlowCode GenerateFlowCode(FlowDefinition pFlow)
        {
            FlowCode ret = new FlowCode();

            SetParentContainer(pFlow.CodeCreator);

            var wfReferencesDict = ReferenceHelper.GetMetadataReferences(pFlow);

            ret.ReferencedAssemblies = wfReferencesDict.Values;


            FlowCodeWriter cw = new FlowCodeWriter();

            FlowBuilderContext context = new FlowBuilderContext(cw, ret.ReferencedAssemblies.ToList());


            cw.AppendLineTop("namespace DynamicGeneratedFlow" + pFlow.Identifier.ToString().ToVariableName() + " {");

            foreach (string import in pFlow.ReferencedNamespaces.Distinct())
            {
                cw.AppendLineTop($"using {import};");
            }

            cw.AppendLineTop();

            cw.WriteIdentifierTagTop(pFlow);
            cw.WriteContainerTagTop(pFlow);

            //Currently idk which letters needs an escape
            cw.AppendLineTop("public class wf_" + pFlow.Name.Replace(" ", "") + " : " + nameof(ICompiledFlow) + "  {");

            cw.AppendLineTop();

            cw.AppendLineTop($"public Guid {INSTANCE_ID_PARAMETER_NAME} = Guid.NewGuid();");

            cw.AppendLineTop();

            if (pFlow.Arguments != null)
                foreach (FlowArguments parameter in pFlow.Arguments)
                {
                    if (!wfReferencesDict.ContainsKey(parameter.Type.Assembly))
                        throw new ArgumentException($"Flow has parameter with type {parameter.Type}, but does not reference assembly {parameter.Type.Assembly.FullName}");

                    string value = $"default({parameter.Type.FullName})";

                    if (parameter.Expression != null && !string.IsNullOrWhiteSpace(parameter.Expression))
                        value = parameter.Expression;

                    cw.AppendLineTop($"public {parameter.Type.FullName} {parameter.Name} = {value};");
                }

            cw.AppendLineTop();

            cw.AppendLineTop("public void Run() { ");


            cw.AppendLineBottom("}"); //Close Run

            cw.AppendLineBottom("}"); //Close Class

            cw.AppendLineBottom("}"); //Close Namespace


            if (pFlow.CodeCreator != null)
            {
                if (pFlow.CodeCreator is IVariableCreator ccv)
                    ccv.Initialize(context, cw);

                pFlow.CodeCreator.ToCode(context, cw);
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
