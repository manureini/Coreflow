namespace Coreflow.Objects
{
    public class FlowInvokeResult
    {
        public FlowCompileResult CompileResult { get; internal set; }

        public ICompiledFlow ExecutedInstance { get; internal set; }
    }
}
