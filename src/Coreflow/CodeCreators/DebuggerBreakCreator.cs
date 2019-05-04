using System;
using System.Diagnostics;
using System.Threading;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class DebuggerBreakCreator : ICodeCreator, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public string Name => "Wait for debugger";

        public string Icon => "fa-bug";

        public string Category => "Basic";

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            pCodeWriter.AppendLineTop("System.Diagnostics.Debugger.Launch();");

            pCodeWriter.AppendLineTop("System.Console.WriteLine(\"Wait for debugger...\");");
            pCodeWriter.AppendLineTop("while (!System.Diagnostics.Debugger.IsAttached) {");
            pCodeWriter.AppendLineTop("System.Threading.Thread.Sleep(500);");
            pCodeWriter.AppendLineTop("}");

            pCodeWriter.AppendLineTop("System.Diagnostics.Debugger.Break();");
        }
    }
}
