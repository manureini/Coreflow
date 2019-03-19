using System;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class TerminateCreator : ICodeCreator, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string Name => "Terminate";

        public string Icon => "fa-times";

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            pCodeWriter.AppendLineTop("return;");
        }
    }
}
