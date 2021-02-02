using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.ParameterVariables;
using System.Collections.Generic;

namespace Coreflow.CodeCreators
{
    public class IfElseCreator : AbstractDualSequenceCreator, IParametrized
    {
        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public IfElseCreator()
        {
        }

        public IfElseCreator(ICodeCreatorContainerCreator pParentContainerCreator) : base(pParentContainerCreator)
        {
        }

        public override string Name => "If Else";

        public override string Icon => "fa-check-double";

        public override string Category => "Basic";

        public override void ToSequenceCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeBuilder)
        {
            pBuilderContext.BuildingContext.Add(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY, IfCreator.CustomDefaultExpressionIfArgument);

            pCodeBuilder.AppendLineTop("if(");
            Arguments[0].ToCode(pBuilderContext, pCodeBuilder);
            pCodeBuilder.AppendLineTop(")");

            pBuilderContext.BuildingContext.Remove(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY);

            AddFirstCodeCreatorsCode(pBuilderContext, pCodeBuilder);

            pCodeBuilder.AppendLineTop("else");

            AddSecondCodeCreatorsCode(pBuilderContext, pCodeBuilder);
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
