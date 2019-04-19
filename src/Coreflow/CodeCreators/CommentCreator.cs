using System;
using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class CommentCreator : ICodeCreatorContainerCreator
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public ICodeCreatorContainerCreator ParentContainerCreator { get; set; }

        public List<List<ICodeCreator>> CodeCreators { get; set; } = new List<List<ICodeCreator>>();

        public int SequenceCount { get; } = 1;

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            pCodeWriter.WriteContainerTagTop(this);
            pCodeWriter.AppendLineTop("//Comment");
        }
    }
}
