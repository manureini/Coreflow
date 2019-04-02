using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.CodeCreators
{
    public abstract class AbstractDualSequenceCreator : ICodeCreatorContainerCreator, IUiDesignable
    {
        public List<List<ICodeCreator>> CodeCreators { get; set; } = new List<List<ICodeCreator>>();

        public ICodeCreatorContainerCreator ParentContainerCreator { get; set; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public virtual string Name => this.GetType().Name;

        public virtual string Icon => "fa-tasks";

        public int SequenceCount { get; } = 2;

        public AbstractDualSequenceCreator()
        {
        }

        public AbstractDualSequenceCreator(ICodeCreatorContainerCreator pParentContainerCreator)
        {
            ParentContainerCreator = pParentContainerCreator;
        }

        public abstract void ToSequenceCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer);


        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeBuilder.WriteIdentifierTagTop(this);
            pCodeBuilder.WriteContainerTagTop(this);


            pCodeBuilder.AppendLineTop("{");
            pCodeBuilder.AppendLineBottom("}");

            ToSequenceCode(pBuilderContext, pCodeBuilder, pContainer);
        }

        protected void AddInitializeCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder)
        {
            foreach (IVariableCreator varCreator in CodeCreators.SelectMany(a => a).Select(a => a as IVariableCreator).Where(a => a != null))
            {
                IVariableCreator existing = WorkflowBuilderHelper.GetVariableCreatorInScope(this, varCreator, c => c.VariableIdentifier == varCreator.VariableIdentifier && pBuilderContext.HasLocalVariableName(c));
                if (existing != null)
                {
                    pBuilderContext.SetLocalVariableName(varCreator, pBuilderContext.GetLocalVariableName(existing));
                    continue;
                }

                varCreator.Initialize(pBuilderContext, pCodeBuilder);
            }
        }

        protected void AddFirstCodeCreatorsCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter)
        {
            if (CodeCreators.Count >= 1)
                foreach (ICodeCreator c in CodeCreators.First())
                {
                    c.ToCode(pBuilderContext, pCodeWriter, this);
                }
        }

        protected void AddSecondCodeCreatorsCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter)
        {
            if (CodeCreators.Count >= 2)
                foreach (ICodeCreator c in CodeCreators.Skip(1).First())
            {
                c.ToCode(pBuilderContext, pCodeWriter, this);
            }
        }
    }
}
