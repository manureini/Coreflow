using Coreflow.Objects;

namespace Coreflow.Interfaces
{
    public interface IVariableCreator : ICodeCreator, IIdentifiable
    {
        void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter);

        string VariableIdentifier { get; }
    }
}
