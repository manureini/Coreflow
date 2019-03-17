using Coreflow.Helper;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Coreflow.Objects
{
    public class WorkflowCode
    {
        public string Code { get; set; }

        public IEnumerable<MetadataReference> ReferencedAssemblies { get; set; }

        public WorkflowCompileResult Compile()
        {
            return WorkflowCompilerHelper.CompileWorkflowCode(this);
        }
    }
}