using Coreflow.Objects;
using System.IO;
using System.Text;

namespace Coreflow.Api.Responses
{
    public class LastCompiledFlowInfo
    {
        public static LastCompiledFlowInfo FromCompileResult(FlowCompileResult pFlowCompileResult)
        {
            if (pFlowCompileResult == null)
                return null;

            return new LastCompiledFlowInfo()
            {
                DllFilePath = pFlowCompileResult.DllFilePath,
                PdbFilePath = pFlowCompileResult.PdbFilePath,
                SourcePath = pFlowCompileResult.SourcePath,
                Successful = pFlowCompileResult.Successful,
                SourceCode = File.ReadAllText(pFlowCompileResult.SourcePath, Encoding.UTF8)
            };
        }

        public bool Successful { get; set; }

        public string DllFilePath { get; set; }

        public string PdbFilePath { get; set; }

        public string SourcePath { get; set; }

        public string SourceCode { get; set; }

        //serializer
        public LastCompiledFlowInfo() { }
    }
}
