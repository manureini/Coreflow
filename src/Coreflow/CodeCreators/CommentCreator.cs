using System;
using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class CommentCreator : ICodeCreatorContainerCreator
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public ICodeCreatorContainerCreator ParentContainerCreator { get; set; }

        public List<ICodeCreator> CodeCreators { get; set; } = new List<ICodeCreator>();

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null)
        {
            pCodeWriter.WriteIdentifierTag(this);
            pCodeWriter.WriteContainerTag(this);
            pCodeWriter.AppendLine("//Comment");
        }
    }
}
