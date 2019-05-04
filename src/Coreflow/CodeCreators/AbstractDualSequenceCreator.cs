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

        public string FactoryIdentifier { get; set; }

        public virtual string Name => this.GetType().Name;

        public virtual string Icon => "fa-tasks";

        public virtual string Category => null;

        public int SequenceCount { get; } = 2;

        protected string RemoveLabelAndCloseBracket
        {
            get
            {
                return "/* remove " + Identifier + "*/ }";
            }
        }


        public AbstractDualSequenceCreator()
        {
        }

        public AbstractDualSequenceCreator(ICodeCreatorContainerCreator pParentContainerCreator)
        {
            ParentContainerCreator = pParentContainerCreator;
        }

        public abstract void ToSequenceCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer);


        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeBuilder.WriteIdentifierTagTop(this);
            pCodeBuilder.WriteContainerTagTop(this);

            pCodeBuilder.AppendLineTop("{");  /* DualContainer */
            pCodeBuilder.AppendLineBottom("}"); /* DualContainer */

            AddInitializeCode(pBuilderContext, pCodeBuilder);
            ToSequenceCode(pBuilderContext, pCodeBuilder, pContainer);
        }

        protected virtual void AddInitializeCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            if (CodeCreators.Count > 0)
                foreach (IVariableCreator varCreator in CodeCreators.SelectMany(a => a).Select(a => a as IVariableCreator).Where(a => a != null))
                {
                    IVariableCreator existing = FlowBuilderHelper.GetVariableCreatorInInitialScope(this, varCreator, c => c.VariableIdentifier == varCreator.VariableIdentifier && pBuilderContext.HasLocalVariableName(c));
                    if (existing != null)
                    {
                        pBuilderContext.SetLocalVariableName(varCreator, pBuilderContext.GetLocalVariableName(existing));
                        continue;
                    }

                    varCreator.Initialize(pBuilderContext, pCodeWriter);
                }
        }

        protected void AddFirstCodeCreatorsCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.AppendLineTop();
            pCodeWriter.AppendLineTop("{"); /* first */

            pCodeWriter.AppendLineBottom(RemoveLabelAndCloseBracket);
            pCodeWriter.AppendLineBottom();

            if (CodeCreators.Count > 0 && CodeCreators[0] != null)
                foreach (ICodeCreator c in CodeCreators[0])
                {
                    AbstractSingleSequenceCreator.ProcessCodeCreator(pBuilderContext, pCodeWriter, c, this);
                }

            pCodeWriter.ReplaceBottom(RemoveLabelAndCloseBracket, "");
            pCodeWriter.AppendLineTop("}"); /* first */
        }

        protected void AddSecondCodeCreatorsCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.AppendLineTop("{"); /* second */

            pCodeWriter.AppendLineBottom("}"); /* second */

            if (CodeCreators.Count > 1 && CodeCreators[1] != null)
                foreach (ICodeCreator c in CodeCreators[1])
                {
                    AbstractSingleSequenceCreator.ProcessCodeCreator(pBuilderContext, pCodeWriter, c, this);
                }
        }

    }
}
