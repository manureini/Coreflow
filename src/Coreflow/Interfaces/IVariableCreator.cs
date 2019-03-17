using Coreflow.Objects;

namespace Coreflow.Interfaces
{
    public interface IVariableCreator : ICodeCreator, IIdentifiable
    {
        void Initialize(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter);

        string VariableIdentifier { get; }
    }
}
