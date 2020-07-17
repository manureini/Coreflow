using System;
using System.Collections.Generic;

namespace Coreflow.Objects
{
    public class FlowCompileResult
    {
        public bool Successful { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public IDictionary<Guid, string> ErrorCodeCreators { get; internal set; }

        internal FlowCompileResult() { }

        public string DllFilePath { get; internal set; }

        public string PdbFilePath { get; internal set; }

        public string SourcePath { get; internal set; }
    }
}
