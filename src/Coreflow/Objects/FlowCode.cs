using Coreflow.Helper;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Coreflow.Objects
{
    public class FlowCode
    {
        public string Code { get; set; }

        public IEnumerable<MetadataReference> ReferencedAssemblies { get; set; }

        public FlowCompileResult Compile()
        {
            return FlowCompilerHelper.CompileFlowCode(this);
        }
    }
}