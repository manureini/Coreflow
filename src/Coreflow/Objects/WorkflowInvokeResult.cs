namespace Coreflow.Objects
{
    public class WorkflowInvokeResult
    {
        public WorkflowCompileResult CompileResult { get; internal set; }

        public ICompiledWorkflow ExecutedInstance { get; internal set; }
    }
}
