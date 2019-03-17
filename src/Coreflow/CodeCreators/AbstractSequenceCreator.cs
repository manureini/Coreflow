using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.CodeCreators
{
    public abstract class AbstractSequenceCreator : ICodeCreatorContainerCreator, IUiDesignable
    {
        public List<ICodeCreator> CodeCreators { get; set; } = new List<ICodeCreator>();

        public ICodeCreatorContainerCreator ParentContainerCreator { get; set; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public virtual string Name => this.GetType().Name;

        public virtual string Icon => "fa-tasks";

        public AbstractSequenceCreator()
        {
        }

        public AbstractSequenceCreator(ICodeCreatorContainerCreator pParentContainerCreator)
        {
            ParentContainerCreator = pParentContainerCreator;
        }

        public abstract void ToSequenceCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer);

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            pCodeBuilder.WriteIdentifierTag(this);
            pCodeBuilder.WriteContainerTag(this);

            pCodeBuilder.AppendLine("{");

            ToSequenceCode(pBuilderContext, pCodeBuilder, pContainer);

            pCodeBuilder.AppendLine("}");
        }

        protected void AddInitializeCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder)
        {
            foreach (IVariableCreator varCreator in CodeCreators.Select(a => a as IVariableCreator).Where(a => a != null))
            {
                IVariableCreator existing = WorkflowBuilderHelper.GetParentVariableCreator(this, c => c.VariableIdentifier == varCreator.VariableIdentifier && pBuilderContext.HasLocalVariableName(c));
                if (existing != null)
                {
                    pBuilderContext.SetLocalVariableName(varCreator, pBuilderContext.GetLocalVariableName(existing));
                    continue;
                }

                varCreator.Initialize(pBuilderContext, pCodeBuilder);
            }
        }

        protected void AddCodeCreatorsCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter)
        {
            foreach (ICodeCreator c in CodeCreators)
            {
                c.ToCode(pBuilderContext, pCodeWriter, this);
            }
        }
    }
}
