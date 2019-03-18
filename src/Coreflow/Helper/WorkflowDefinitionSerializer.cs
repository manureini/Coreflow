using Coreflow.Objects;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Content;
using ExtendedXmlSerializer.ExtensionModel.Xml;

using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Coreflow.Helper
{
    public static class WorkflowDefinitionSerializer
    {
        private static IExtendedXmlSerializer Serializer = new ConfigurationContainer()
                                                            .UseOptimizedNamespaces()
                                                            .CustomSerializer<WorkflowArguments, WorkflowArgumentSerializer>()
                                                            .CustomSerializer<CodeCreatorParameter, CodeCreatorParameterSerializer>()
                                                            .CustomSerializer<Assembly, AssemblySerializer>()
                                                            .ConfigureType<WorkflowDefinition>().Member(x => x.Coreflow).Ignore()
                                                            .EnableReferences()
                                                            .Create();

        //                               .Emit(EmitBehaviors.Assigned) does currently not work with lists


        public static string Serialize(WorkflowDefinition pWorkflowDefinition)
        {
            return Serializer.Serialize(new XmlWriterSettings() { Indent = true }, pWorkflowDefinition);
        }


        public static WorkflowDefinition DeSerialize(string pWorkflowDefinition, Coreflow pCoreflow)
        {
            WorkflowDefinition ret = Serializer.Deserialize<WorkflowDefinition>(pWorkflowDefinition);
            ret.Coreflow = pCoreflow;
            return ret;
        }
    }

    public class WorkflowArgumentSerializer : IExtendedXmlCustomSerializer<WorkflowArguments>
    {
        public WorkflowArguments Deserialize(XElement element)
        {
            var xName = element.Attribute("Name");
            var xType = element.Attribute("Type");
            var xDirection = element.Attribute("Direction");
            var xExpression = element.Attribute("Expression");

            VariableDirection direction = Enum.Parse<VariableDirection>(xDirection.Value);
            return new WorkflowArguments(xName.Value, Type.GetType(xType.Value), direction, xExpression?.Value);
        }

        public void Serializer(XmlWriter writer, WorkflowArguments obj)
        {
            writer.WriteAttributeString("Name", obj.Name);
            writer.WriteAttributeString("Type", obj.Type.AssemblyQualifiedName);
            writer.WriteAttributeString("Direction", obj.Direction.ToString());
            writer.WriteAttributeString("Expression", obj.Expression);
        }
    }

    public class CodeCreatorParameterSerializer : IExtendedXmlCustomSerializer<CodeCreatorParameter>
    {
        public CodeCreatorParameter Deserialize(XElement element)
        {
            var xName = element.Attribute("Name");
            var xDisplayName = element.Attribute("DisplayName");
            var xType = element.Attribute("Type");
            var xCategory = element.Attribute("Category");
            var xDirection = element.Attribute("Direction");

            return new CodeCreatorParameter(xName.Value, xDisplayName.Value, Type.GetType(xType.Value), xCategory.Value, Enum.Parse<VariableDirection>(xDirection.Value));
        }

        public void Serializer(XmlWriter writer, CodeCreatorParameter obj)
        {
            writer.WriteAttributeString("Name", obj.Name);
            writer.WriteAttributeString("DisplayName", obj.DisplayName);
            writer.WriteAttributeString("Type", obj.Type.AssemblyQualifiedName);
            writer.WriteAttributeString("Category", obj.Category);
            writer.WriteAttributeString("Direction", obj.Direction.ToString());
        }
    }

    public class AssemblySerializer : IExtendedXmlCustomSerializer<Assembly>
    {
        public Assembly Deserialize(XElement xElement)
        {
            string xFullName = xElement.Attribute("FullName").Value;
            Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == xFullName);
            return asm;
        }

        public void Serializer(XmlWriter xmlWriter, Assembly obj)
        {
            xmlWriter.WriteAttributeString("FullName", obj.FullName);
        }
    }

    /*
    public class ICodeActivityCreatorSerializer : IExtendedXmlCustomSerializer<ICodeActivityCreator>
    {
   //     readonly IParameterizedSource<TypeInfo, IExtendedXmlCustomSerializer> _serializers;

   //     public ICodeActivityCreatorSerializer(ICustomXmlSerializers serializers) => _serializers = serializers;
   
        public void Serializer(XmlWriter xmlWriter, ICodeActivityCreator obj)
        {

            xmlWriter.WriteAttributeString("CodeActivityType", obj.CodeActivityType.FullName);
            xmlWriter.WriteAttributeString("LocalObjectName", obj.LocalObjectName);

         

            //    _serializers.Get(obj.mParameterMapping.GetType()).Serializer(xmlWriter, obj);
        }

        ICodeActivityCreator IExtendedXmlCustomSerializer<ICodeActivityCreator>.Deserialize(XElement xElement)
        {
            string acivityType = xElement.Attribute("CodeActivityType").Value;
            string localObjectName = xElement.Attribute("LocalObjectName").Value;

            Type constructedType = typeof(CodeActivityCreator<>).MakeGenericType(Type.GetType(acivityType));

            ICodeActivityCreator activityCreator = Activator.CreateInstance(constructedType) as ICodeActivityCreator;
            activityCreator.LocalObjectName = localObjectName;

            return activityCreator;
        }
    }
    */
    /*
    sealed class ICodeActivityCreatorSerializer : ISerializer<ICodeActivityCreator>
    {
        readonly ISerializer mParameterMappingSerializer;

        public ICodeActivityCreatorSerializer(IContents contents) : this(contents.Get(typeof(Dictionary<string, ICodeCreator>))) { }

        ICodeActivityCreatorSerializer(ISerializer subject) => mParameterMappingSerializer = subject;

        public ICodeActivityCreator Get(IFormatReader parameter)
        {

            var xml = parameter.Get() as XmlReader;

            string acivityType = xml.GetAttribute("CodeActivityType");
            string localObjectName = xml.GetAttribute("LocalObjectName");

            Type constructedType = typeof(CodeActivityCreator<>).MakeGenericType(Type.GetType(acivityType));

            ICodeActivityCreator activityCreator = Activator.CreateInstance(constructedType) as ICodeActivityCreator;
            activityCreator.LocalObjectName = localObjectName;


            xml.Read();
            xml.MoveToContent();

            //    xml.Read();

            activityCreator.mParameterMapping = mParameterMappingSerializer.Get(parameter) as Dictionary<string, ICodeCreator>;


            return activityCreator;
        }

        public void Write(IFormatWriter writer, ICodeActivityCreator instance)
        {
            var xml = writer.Get() as XmlWriter;

            xml.WriteAttributeString("CodeActivityType", instance.CodeActivityType.FullName);
            xml.WriteAttributeString("LocalObjectName", instance.LocalObjectName);

            xml.WriteStartElement("ParameterMapping");
            mParameterMappingSerializer.Write(writer, instance.mParameterMapping);
            xml.WriteEndElement();
        }
    } */

}
