using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Coreflow.Objects
{
    public class FlowCompileResult
    {
        public FlowCode FlowCode { get; internal set; }

        public bool Successful { get; internal set; }

        public MemoryStream ResultAssembly { get; internal set; }

        public MemoryStream ResultSymbols { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public IDictionary<Guid, string> ErrorCodeCreators { get; internal set; }

        public FlowInstanceFactory InstanceFactory { get; internal set; }

        internal FlowCompileResult() { }
    }
}
