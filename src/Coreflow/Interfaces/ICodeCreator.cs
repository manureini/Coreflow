using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow
{
    public interface ICodeCreator : IIdentifiable
    {
        void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null);
    }
}
