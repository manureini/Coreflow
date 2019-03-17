using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow
{
    internal class WorkflowBuilderHelper
    {
        private const string INSTANCE_ID_PARAMETER_NAME = "InstanceId";

        public static IEnumerable<WorkflowCode> GenerateWorkflowCode(IEnumerable<WorkflowDefinition> pWorkflows)
        {
            return pWorkflows.Select(w => GenerateWorkflowCode(w));
        }

        public static WorkflowCode GenerateWorkflowCode(WorkflowDefinition pWorkflow)
        {
            WorkflowCode ret = new WorkflowCode();

            var wfReferencesDict = ReferenceHelper.GetMetadataReferences(pWorkflow);

            ret.ReferencedAssemblies = wfReferencesDict.Values;


            WorkflowBuilderContext context = new WorkflowBuilderContext();


            WorkflowCodeWriter cw = new WorkflowCodeWriter();

            cw.AppendLine("namespace DynamicGeneratedWorkflow" + pWorkflow.Identifier.ToString().ToVariableName() + " {");

            foreach (string import in pWorkflow.ReferencedNamespaces.Distinct())
            {
                cw.AppendLine($"using {import};");
            }

            cw.AppendLine();

            cw.WriteIdentifierTag(pWorkflow);
            cw.WriteContainerTag(pWorkflow);

            //Currently idk which letters needs an escape
            cw.AppendLine("public class wf_" + pWorkflow.Name.Replace(" ", "") + " : " + nameof(ICompiledWorkflow) + "  {");

            cw.AppendLine();

            cw.AppendLine($"public Guid {INSTANCE_ID_PARAMETER_NAME} = Guid.NewGuid();");

            cw.AppendLine();

            if (pWorkflow.Arguments != null)
                foreach (WorkflowArguments parameter in pWorkflow.Arguments)
                {
                    if (!wfReferencesDict.ContainsKey(parameter.Type.Assembly))
                        throw new ArgumentException($"Workflow has parameter with type {parameter.Type}, but does not reference assembly {parameter.Type.Assembly.FullName}");

                    string value = $"default({parameter.Type.FullName})";

                    if (parameter.DefaultExpression != null && !string.IsNullOrWhiteSpace(parameter.DefaultExpression))
                        value = parameter.DefaultExpression;

                    cw.AppendLine($"public {parameter.Type.FullName} {parameter.Name} = {value};");
                }

            cw.AppendLine();

            cw.AppendLine("public void Run() { ");

            if (pWorkflow.CodeCreator != null)
            {
                if (pWorkflow.CodeCreator is IVariableCreator ccv)
                    ccv.Initialize(context, cw);

                pWorkflow.CodeCreator.ToCode(context, cw);
            }

            cw.AppendLine("}"); //Close Run

            cw.AppendLine("}"); //Close Class

            cw.AppendLine("}"); //Close Namespace

            ret.Code = cw.ToString();
            return ret;
        }

        public static IVariableCreator GetParentVariableCreator(ICodeCreatorContainerCreator pContainer, Func<IVariableCreator, bool> pFilter)
        {
            if (pContainer == null)
                return null;

            return GetParentVariableCreator(pContainer.ParentContainerCreator, pFilter) ?? pContainer.CodeCreators.Select(v => v as IVariableCreator).Where(v => v != null).FirstOrDefault(pFilter);
        }
    }
}
