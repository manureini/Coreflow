using Coreflow.Objects;
using Coreflow.Runtime;
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
    public static class FlowDefinitionSerializer
    {
        private static IExtendedXmlSerializer Serializer = new ConfigurationContainer()
                                                            .UseOptimizedNamespaces()
                                                            .CustomSerializer<FlowArgument, FlowArgumentSerializer>()
                                                            .CustomSerializer<CodeCreatorParameter, CodeCreatorParameterSerializer>()
                                                            .CustomSerializer<Assembly, AssemblySerializer>()
                                                            .ConfigureType<FlowDefinition>().Member(x => x.Coreflow).Ignore()
                                                            .EnableReferences()
                                                            .Create();

        //                               .Emit(EmitBehaviors.Assigned) does currently not work with lists


        public static string Serialize(IFlowDefinition pFlowDefinition)
        {
            return Serializer.Serialize(new XmlWriterSettings() { Indent = true }, pFlowDefinition);
        }


        public static FlowDefinition Deserialize(string pFlowDefinition, CoreflowRuntime pCoreflow)
        {
            FlowDefinition ret = Serializer.Deserialize<FlowDefinition>(pFlowDefinition);
            ret.Coreflow = pCoreflow;
            return ret;
        }
    }

    public class FlowArgumentSerializer : IExtendedXmlCustomSerializer<FlowArgument>
    {
        public FlowArgument Deserialize(XElement element)
        {
            var xName = element.Attribute("Name");
            var xType = element.Attribute("Type");
            var xDirection = element.Attribute("Direction");
            var xExpression = element.Attribute("Expression");

            VariableDirection direction = VariableDirectionHelper.Parse(xDirection.Value);

            return new FlowArgument(xName.Value, Type.GetType(xType.Value), direction, xExpression?.Value);
        }

        public void Serializer(XmlWriter writer, FlowArgument obj)
        {
            writer.WriteAttributeString("Name", obj.Name);
            writer.WriteAttributeString("Type", obj.Type?.AssemblyQualifiedName);
            writer.WriteAttributeString("Direction", obj.Direction.ToString());
            writer.WriteAttributeString("Expression", obj.Expression);
        }
    }

    /*
    public class InputExpressionCreatorSerializer : IExtendedXmlCustomSerializer<InputExpressionCreator>
    {
        public InputExpressionCreator Deserialize(XElement xElement)
        {
            var xIdentifier = xElement.Attribute("Identifier");
            var xVariableIdentifier = xElement.Attribute("VariableIdentifier");
            var xCode = xElement.Attribute("Code");
            var xName = xElement.Attribute("Name");
            var xType = xElement.Attribute("Type");

            return new InputExpressionCreator()
            {
                Identifier = Guid.Parse(xIdentifier.Value),
                VariableIdentifier = xVariableIdentifier.Value,
                Code = xCode.Value,
                Name = xName.Value,
                Type = xType.Value
            };
        }

        public void Serializer(XmlWriter xmlWriter, InputExpressionCreator obj)
        {
            xmlWriter.WriteAttributeString("Identifier", obj.Identifier.ToString());
            xmlWriter.WriteAttributeString("VariableIdentifier", obj.VariableIdentifier);
            xmlWriter.WriteAttributeString("Code", obj.Code);
            xmlWriter.WriteAttributeString("Name", obj.Name);
            xmlWriter.WriteAttributeString("Type", obj.Type.AssemblyQualifiedName);
        }
    }*/

    public class CodeCreatorParameterSerializer : IExtendedXmlCustomSerializer<CodeCreatorParameter>
    {
        public CodeCreatorParameter Deserialize(XElement element)
        {
            var xName = element.Attribute(nameof(CodeCreatorParameter.Name));
            var xDisplayName = element.Attribute(nameof(CodeCreatorParameter.DisplayName));
            var xType = element.Attribute(nameof(CodeCreatorParameter.Type));
            var xCategory = element.Attribute(nameof(CodeCreatorParameter.Category));
            var xDirection = element.Attribute(nameof(CodeCreatorParameter.Direction));
            var xDefaultValueCode = element.Attribute(nameof(CodeCreatorParameter.DefaultValueCode));

            return new CodeCreatorParameter(
                xName.Value,
                xDisplayName.Value,
                TypeHelper.SearchType(xType.Value),
                xCategory.Value,
                VariableDirectionHelper.Parse(xDirection.Value),
                xDefaultValueCode.Value
                );
        }

        public void Serializer(XmlWriter writer, CodeCreatorParameter obj)
        {
            writer.WriteAttributeString(nameof(CodeCreatorParameter.Name), obj.Name);
            writer.WriteAttributeString(nameof(CodeCreatorParameter.DisplayName), obj.DisplayName);
            writer.WriteAttributeString(nameof(CodeCreatorParameter.Type), obj.Type.AssemblyQualifiedName);
            writer.WriteAttributeString(nameof(CodeCreatorParameter.Category), obj.Category);
            writer.WriteAttributeString(nameof(CodeCreatorParameter.Direction), obj.Direction.ToString());
            writer.WriteAttributeString(nameof(CodeCreatorParameter.DefaultValueCode), obj.DefaultValueCode);
        }
    }

    public class AssemblySerializer : IExtendedXmlCustomSerializer<Assembly>
    {
        public Assembly Deserialize(XElement xElement)
        {
            string xFullName = xElement.Attribute("FullName").Value;
            Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == xFullName);

            if (asm == null)
            {
                Assembly.Load(xFullName);
            }

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
