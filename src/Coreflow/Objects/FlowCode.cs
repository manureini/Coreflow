using Coreflow.Helper;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Coreflow.Objects
{
    public class FlowCode
    {
        public string Code { get; set; }

        public string FormattedCode => FlowBuilderHelper.FormatCode(Code);

        public IEnumerable<MetadataReference> ReferencedAssemblies { get; set; }

        public FlowDefinition Definition { get; set; }

        public FlowCompileResult Compile()
        {
            return FlowCompilerHelper.CompileFlowCode(Code, ReferencedAssemblies);
        }
    }
}