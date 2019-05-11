using Coreflow.Objects;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Xml;

namespace Coreflow.Helper
{
    public static class FlowInstanceSerializer
    {
        private static IExtendedXmlSerializer Serializer = new ConfigurationContainer()
                                                            .UseOptimizedNamespaces()
                                                            .EnableReferences()
                                                            .Create();

        public static string Serialize(FlowInstance pFlowInstance)
        {
            return Serializer.Serialize(new XmlWriterSettings() { Indent = true }, pFlowInstance);
        }

        public static FlowInstance Deserialize(string pFlowInstance)
        {
            return Serializer.Deserialize<FlowInstance>(pFlowInstance);
        }
    }
}
