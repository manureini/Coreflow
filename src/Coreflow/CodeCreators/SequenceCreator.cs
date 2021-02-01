using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class SequenceCreator : AbstractSingleSequenceCreator
    {
        public SequenceCreator() : base()
        {
        }

        public SequenceCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "Sequence";

        public override string Category => "Basic";

        public override void ToSequenceCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            AddCodeCreatorsCode(pBuilderContext, pCodeWriter);
        }
    }
}
