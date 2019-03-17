using Coreflow.Storage;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Web.Controllers
{
    public class WorkflowDefinitionModelMappingHelper
    {
        public static WorkflowDefinitionModel GenerateModel(WorkflowDefinition pWorkflowDefinition)
        {
            WorkflowDefinitionModel ret = new WorkflowDefinitionModel()
            {
                Name = pWorkflowDefinition.Name,
                ReferencedNamespaces = pWorkflowDefinition.ReferencedNamespaces,
                ReferencedAssemblies = pWorkflowDefinition.ReferencedAssemblies.Select(a => a.FullName).ToList(),
                Identifier = pWorkflowDefinition.Identifier,
                CodeCreatorModel = CodeCreatorModelHelper.CreateModel(pWorkflowDefinition.CodeCreator, null, pWorkflowDefinition),
                CodeCreators = Program.CoreflowInstance.CodeCreatorStorage.GetAllCodeCreators().ToDictionary(k => k.Key.FullName, v => CodeCreatorModelHelper.CreateModel(v.Value, null, null)),
                Arguments = pWorkflowDefinition.Arguments
            };

            foreach (CodeCreatorModel ccm in ret.CodeCreators.Values)
            {
                ccm.Identifier = Guid.Empty;
                ccm.Arguments?.ForEach(v => v.Guid = Guid.Empty);
            }

            return ret;
        }

        public static WorkflowDefinition GenerateWorkflowDefinition(WorkflowDefinitionModel pWorkflowDefinitionModel)
        {
            WorkflowDefinition ret = new WorkflowDefinition()
            {
                Name = pWorkflowDefinitionModel.Name,
                ReferencedNamespaces = pWorkflowDefinitionModel.ReferencedNamespaces,
                Identifier = pWorkflowDefinitionModel.Identifier,
                Arguments = pWorkflowDefinitionModel.Arguments
            };

            ret.ReferencedAssemblies = new List<Assembly>();

            foreach (string assemblyFullName in pWorkflowDefinitionModel.ReferencedAssemblies)
            {
                Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyFullName);

                if (asm == null)
                    throw new Exception($"Assembly '{assemblyFullName}' not found!");

                ret.ReferencedAssemblies.Add(asm);
            }

            ret.CodeCreator = CodeCreatorModelHelper.CreateCode(pWorkflowDefinitionModel.CodeCreatorModel, ret);

            return ret;
        }
    }
}
