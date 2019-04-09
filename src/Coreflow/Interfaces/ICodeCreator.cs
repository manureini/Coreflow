using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow
{
    public interface ICodeCreator : IIdentifiable
    {
        void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null);
    }
}
