using Coreflow.Interfaces;
using Coreflow.Objects;
using System.Collections.Generic;

namespace Coreflow.CodeCreators
{
    public class IfCreator : AbstractSingleSequenceCreator, IParametrized
    {
        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public IfCreator()
        {
        }

        public IfCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "If";

        public override string Icon => "fa-check";

        public override void ToSequenceCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            AddInitializeCode(pBuilderContext, pCodeBuilder);

            pCodeBuilder.AppendLineTop("if(");

            Arguments[0].ToCode(pBuilderContext, pCodeBuilder, pContainer);

            pCodeBuilder.AppendLineTop(") {");

            pCodeBuilder.AppendLineBottom("}");

            AddCodeCreatorsCode(pBuilderContext, pCodeBuilder);
        }

        public CodeCreatorParameter[] GetParameters()
        {
            return new[] { new CodeCreatorParameter() {
                 Direction = VariableDirection.In,
                 Name = "Expression",
                 Type = typeof(CSharpCode)
                }
            };
        }
    }
}
