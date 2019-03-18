using Coreflow.Helper;
using Coreflow.Objects;
using Coreflow.Web.Models;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Content;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System;
using System.Xml;
using System.Xml.Linq;

namespace Coreflow.Web.Controllers
{
    public static class WorkflowDefinitionModelSerializer
    {
        private static IExtendedXmlSerializer Serializer = new ConfigurationContainer()
                                             .Emit(EmitBehaviors.Always)
                                             .CustomSerializer<WorkflowArguments, WorkflowArgumentSerializer>()
                                             .CustomSerializer<CodeCreatorParameterModel, CodeCreatorParameterModelSerializer>()
                                             .EnableReferences()
                                             .Create();

        public static string Serialize(WorkflowDefinitionModel pWorkflowDefinition)
        {
            return Serializer.Serialize(new XmlWriterSettings() { Indent = true }, pWorkflowDefinition);
        }

        public static WorkflowDefinitionModel DeSerialize(string pWorkflowDefinition)
        {
            return Serializer.Deserialize<WorkflowDefinitionModel>(pWorkflowDefinition);
        }
    }

    public class CodeCreatorParameterModelSerializer : IExtendedXmlCustomSerializer<CodeCreatorParameterModel>
    {
        public CodeCreatorParameterModel Deserialize(XElement element)
        {
            var xName = element.Attribute("Name");
            var xDisplayName = element.Attribute("DisplayName");
            var xType = element.Attribute("Type");
            var xCategory = element.Attribute("Category");
            var xDirection = element.Attribute("Direction");
            var xIndex = element.Attribute("Index");

            return new CodeCreatorParameterModel(xName.Value, xDisplayName.Value, Type.GetType(xType.Value), xCategory.Value, Enum.Parse<VariableDirection>(xDirection.Value), Convert.ToInt32(xIndex.Value));
        }

        public void Serializer(XmlWriter writer, CodeCreatorParameterModel obj)
        {
            writer.WriteAttributeString("Name", obj.Name);
            writer.WriteAttributeString("DisplayName", obj.DisplayName);
            writer.WriteAttributeString("Type", obj.Type.AssemblyQualifiedName);
            writer.WriteAttributeString("Category", obj.Category);
            writer.WriteAttributeString("Direction", obj.Direction.ToString());
            writer.WriteAttributeString("Index", obj.Index.ToString());
        }
    }
}
