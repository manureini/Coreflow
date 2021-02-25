using Coreflow.Objects;
using System.Text.Json;

namespace Coreflow.Helper
{
    public static class FlowInstanceSerializer
    {
        public static string Serialize(FlowInstance pFlowInstance)
        {
            return JsonSerializer.Serialize(pFlowInstance, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }

        public static FlowInstance Deserialize(string pFlowInstance)
        {
            return JsonSerializer.Deserialize<FlowInstance>(pFlowInstance);
        }
    }
}
