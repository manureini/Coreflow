using Coreflow.Objects;
using Coreflow.Runtime;

namespace Coreflow.Interfaces
{
    public interface ICodeCreator : IIdentifiable
    {
        void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null);
    }
}
