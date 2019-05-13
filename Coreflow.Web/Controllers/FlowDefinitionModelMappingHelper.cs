using Coreflow.Web.Models;
using System;
using System.Collections.Generic;

namespace Coreflow.Web.Controllers
{
    public class FlowDefinitionModelMappingHelper
    {
        public static FlowDefinitionModel GenerateModel(FlowDefinition pFlowDefinition)
        {
            FlowDefinitionModel ret = new FlowDefinitionModel()
            {
                Name = pFlowDefinition.Name,
                Icon = pFlowDefinition.Icon,
                ReferencedNamespaces = pFlowDefinition.ReferencedNamespaces,
                Identifier = pFlowDefinition.Identifier,
                CodeCreatorModel = CodeCreatorModelHelper.CreateModel(pFlowDefinition.CodeCreator, null, pFlowDefinition),
                Arguments = pFlowDefinition.Arguments ?? new List<Objects.FlowArguments>()
            };

            if (pFlowDefinition.Metadata != null && pFlowDefinition.Metadata.ContainsKey(Guid.Empty))
            {
                var flowMetadata = pFlowDefinition.Metadata[Guid.Empty];

                if (flowMetadata.ContainsKey(nameof(FlowDefinitionModel.Note)))
                    ret.Note = (string)flowMetadata[nameof(FlowDefinitionModel.Note)];

            }

            return ret;
        }

        public static FlowDefinition GenerateFlowDefinition(FlowDefinitionModel pFlowDefinitionModel)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            FlowDefinition ret = new FlowDefinition()
            {
                Name = pFlowDefinitionModel.Name,
                Icon = pFlowDefinitionModel.Icon,
                ReferencedNamespaces = pFlowDefinitionModel.ReferencedNamespaces,
                Identifier = pFlowDefinitionModel.Identifier,
                Arguments = pFlowDefinitionModel.Arguments,
            };
#pragma warning restore CS0618 // Type or member is obsolete

            if (!string.IsNullOrWhiteSpace(pFlowDefinitionModel.Note))
            {
                ret.Metadata = new Dictionary<Guid, Dictionary<string, object>>();
                ret.Metadata.Add(Guid.Empty, new Dictionary<string, object>());
                ret.Metadata[Guid.Empty].Add(nameof(FlowDefinitionModel.Note), pFlowDefinitionModel.Note);
            }

            ret.Coreflow = Program.CoreflowInstance;
            ret.CodeCreator = CodeCreatorModelHelper.CreateCode(pFlowDefinitionModel.CodeCreatorModel, ret);

            return ret;
        }
    }
}
