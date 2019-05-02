using Coreflow.Interfaces;
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
