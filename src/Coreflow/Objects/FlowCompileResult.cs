using System;
using System.Collections.Generic;
using System.IO;

namespace Coreflow.Objects
{
    public class FlowCompileResult
    {     
        public bool Successful { get; internal set; }

        public MemoryStream ResultAssembly { get; internal set; }

        public MemoryStream ResultSymbols { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public IDictionary<Guid, string> ErrorCodeCreators { get; internal set; }

        public FlowInstanceFactory InstanceFactory { get; internal set; }

        internal FlowCompileResult() { }
    }
}
