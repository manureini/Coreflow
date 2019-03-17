using Coreflow.Interfaces;
using Coreflow.Objects;
using System.Collections.Generic;

namespace Coreflow.CodeCreators
{
    public class ForLoopCreator : AbstractSequenceCreator, IParametrized
    {
        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public ForLoopCreator()
        {
        }

        public ForLoopCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "For Loop";


        public override void ToSequenceCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeBuilder, ICodeCreatorContainerCreator pContainer)
        {
            AddInitializeCode(pBuilderContext, pCodeBuilder);

            pCodeBuilder.AppendLine("for(");

            Arguments[0].ToCode(pBuilderContext, pCodeBuilder, pContainer);

            pCodeBuilder.AppendLine(") {");

            AddCodeCreatorsCode(pBuilderContext, pCodeBuilder);

            pCodeBuilder.AppendLine("}");
        }

        public CodeCreatorParameter[] GetParameters()
        {
            return new[] { new CodeCreatorParameter() {
                 Direction = ParameterDirection.In,
                 Name = "Expression",
                 Type = typeof(ICSharpCode)
                }
            };
        }
    }
}
