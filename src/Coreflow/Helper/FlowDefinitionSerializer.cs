using Coreflow.Helper.Serialization;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Runtime;
using System;

using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace Coreflow.Helper
{
    public static class FlowDefinitionSerializer
    {
        public static JsonSerializerOptions mJsonSerializerOptions;

        static FlowDefinitionSerializer()
        {
            mJsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true
            };
            mJsonSerializerOptions.Converters.Add(new TypeConverter());
            mJsonSerializerOptions.Converters.Add(new TypedConverter<ICodeCreator>());
            mJsonSerializerOptions.Converters.Add(new TypedConverter<IArgument>());
        }

        public static string Serialize(IFlowDefinition pFlowDefinition)
        {
            FlowDefinition flow = pFlowDefinition as FlowDefinition;
            var ret = JsonSerializer.Serialize(flow, mJsonSerializerOptions);
            return ret;
        }

        public static FlowDefinition Deserialize(string pFlowDefinition, CoreflowRuntime pCoreflow)
        {
            FlowDefinition ret = JsonSerializer.Deserialize<FlowDefinition>(pFlowDefinition, mJsonSerializerOptions);
            ret.Coreflow = pCoreflow;
            return ret;
        }
    }
}