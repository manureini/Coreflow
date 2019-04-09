using Coreflow.Storage;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Web.Controllers
{
    public class FlowDefinitionModelMappingHelper
    {
        public static FlowDefinitionModel GenerateModel(FlowDefinition pFlowDefinition)
        {
            FlowDefinitionModel ret = new FlowDefinitionModel()
            {
                Name = pFlowDefinition.Name,
                ReferencedNamespaces = pFlowDefinition.ReferencedNamespaces,
                ReferencedAssemblies = pFlowDefinition.ReferencedAssemblies.Select(a => a.FullName).ToList(),
                Identifier = pFlowDefinition.Identifier,
                CodeCreatorModel = CodeCreatorModelHelper.CreateModel(pFlowDefinition.CodeCreator, null, pFlowDefinition),
                CodeCreators = Program.CoreflowInstance.CodeCreatorStorage.GetAllCodeCreators().ToDictionary(k => k.Key.FullName, v => CodeCreatorModelHelper.CreateModel(v.Value, null, null)),
                Arguments = pFlowDefinition.Arguments
            };

            foreach (CodeCreatorModel ccm in ret.CodeCreators.Values)
            {
                ccm.Identifier = Guid.Empty;
                ccm.Arguments?.ForEach(v => v.Guid = Guid.Empty);
            }

            return ret;
        }

        public static FlowDefinition GenerateFlowDefinition(FlowDefinitionModel pFlowDefinitionModel)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            FlowDefinition ret = new FlowDefinition()
            {
                Name = pFlowDefinitionModel.Name,
                ReferencedNamespaces = pFlowDefinitionModel.ReferencedNamespaces,
                Identifier = pFlowDefinitionModel.Identifier,
                Arguments = pFlowDefinitionModel.Arguments
            };
#pragma warning restore CS0618 // Type or member is obsolete

            ret.ReferencedAssemblies = new List<Assembly>();

            foreach (string assemblyFullName in pFlowDefinitionModel.ReferencedAssemblies)
            {
                Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyFullName);

                if (asm == null)
                    throw new Exception($"Assembly '{assemblyFullName}' not found!");

                ret.ReferencedAssemblies.Add(asm);
            }

            ret.CodeCreator = CodeCreatorModelHelper.CreateCode(pFlowDefinitionModel.CodeCreatorModel, ret);

            return ret;
        }
    }
}
