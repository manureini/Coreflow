using Coreflow.Objects;
using Coreflow.Runtime;

namespace Coreflow.Interfaces
{
    public interface IVariableCreator : ICodeCreator, IIdentifiable
    {
        void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter);

        string VariableIdentifier { get; }
    }
}
