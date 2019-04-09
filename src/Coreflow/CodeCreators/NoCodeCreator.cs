using System;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class NoCodeCreator : ICodeCreator
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string Name => "Nop";

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
        }
    }
}
