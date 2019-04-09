using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.CodeCreators
{
    public abstract class AbstractSingleSequenceCreator : ICodeCreatorContainerCreator, IUiDesignable
    {
        public List<List<ICodeCreator>> CodeCreators { get; set; } = new List<List<ICodeCreator>>();

        public ICodeCreatorContainerCreator ParentContainerCreator { get; set; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public virtual string Name => this.GetType().Name;

        public virtual string Icon => "fa-tasks";

        public int SequenceCount { get; } = 1;

        public AbstractSingleSequenceCreator()
        {
        }

        public AbstractSingleSequenceCreator(ICodeCreatorContainerCreator pParentContainerCreator)
        {
            ParentContainerCreator = pParentContainerCreator;
        }

        public abstract void ToSequenceCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer);

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeBuilder.WriteIdentifierTagTop(this);
            pCodeBuilder.WriteContainerTagTop(this);

            pCodeBuilder.AppendLineTop("{");

            pCodeBuilder.AppendLineBottom("}");

            ToSequenceCode(pBuilderContext, pCodeBuilder, pContainer);
        }

        protected void AddInitializeCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder)
        {
            foreach (IVariableCreator varCreator in CodeCreators.First().Select(a => a as IVariableCreator).Where(a => a != null))
            {
                IVariableCreator existing = FlowBuilderHelper.GetVariableCreatorInScope(this, varCreator, c => c.VariableIdentifier == varCreator.VariableIdentifier && pBuilderContext.HasLocalVariableName(c));
                if (existing != null)
                {
                    pBuilderContext.SetLocalVariableName(varCreator, pBuilderContext.GetLocalVariableName(existing));
                    continue;
                }

                varCreator.Initialize(pBuilderContext, pCodeBuilder);
            }
        }

        protected void AddCodeCreatorsCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            if (CodeCreators.Count >= 1)
                foreach (ICodeCreator c in CodeCreators.First())
                {
                    c.ToCode(pBuilderContext, pCodeWriter, this);
                }
        }
    }
}
