using Coreflow.Interfaces;
using Coreflow.Objects;
using System.Collections.Generic;

namespace Coreflow.CodeCreators
{
    public class ForLoopCreator : AbstractSingleSequenceCreator, IParametrized
    {
        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public ForLoopCreator()
        {
        }

        public ForLoopCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "For Loop";


        public override void ToSequenceCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            pCodeBuilder.AppendLineTop("for(");
            Arguments[0].ToCode(pBuilderContext, pCodeBuilder, pContainer);
            pCodeBuilder.AppendLineTop(")");

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
