using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class SequenceCreator : AbstractSequenceCreator
    {
        public SequenceCreator() : base()
        {
        }

        public SequenceCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "Sequence";

        public override void ToSequenceCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer)
        {
            AddInitializeCode(pBuilderContext, pCodeWriter);
            AddCodeCreatorsCode(pBuilderContext, pCodeWriter);
        }
    }
}
