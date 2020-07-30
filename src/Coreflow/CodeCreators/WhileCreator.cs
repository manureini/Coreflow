using Coreflow.Interfaces;
using Coreflow.Objects;
using System.Collections.Generic;

namespace Coreflow.CodeCreators
{
    public class WhileCreator : AbstractSingleSequenceCreator, IParametrized
    {
        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public WhileCreator()
        {
        }

        public WhileCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "While";

        public override string Icon => "fa-level-down-alt";

        public override string Category => "Basic";

        public override void ToSequenceCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pFlowCodeWriter, ICodeCreatorContainerCreator pContainer)
        {
            pFlowCodeWriter.AppendLineTop("while (");
            Arguments[0].ToCode(pBuilderContext, pFlowCodeWriter, pContainer);
            pFlowCodeWriter.AppendLineTop(")");

            AddCodeCreatorsCode(pBuilderContext, pFlowCodeWriter);
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
