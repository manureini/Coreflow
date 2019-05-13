using Coreflow.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
