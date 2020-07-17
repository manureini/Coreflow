using Coreflow.Runtime;
using Microsoft.Extensions.Logging;

namespace Coreflow.Objects
{
    public static class FlowCodeWriterExtensions
    {

        public static void AppendLoggingCode(this FlowCodeWriter pFlowCodeWriter, LogLevel pLogLevel, string pMessage)
        {
            pFlowCodeWriter.AppendLineTop($"global::Microsoft.Extensions.Logging.LoggerExtensions.Log{pLogLevel}({nameof(ICompiledFlow.Logger)}, \"{pMessage}\");");

        }

    }
}
