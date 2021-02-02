using System;
using System.Collections.Generic;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.ParameterVariables;

namespace Coreflow.CodeCreators
{
    public class AssignCreator : ICodeCreator, IParametrized, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public string Name => "Assign";

        public string Icon => "fa-equals";

        public string Category => "Basic";

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeWriter.WriteIdentifierTagTop(this);

            pBuilderContext.BuildingContext.Add(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY, "object noName_" + Identifier.ToString().ToVariableName());
            Arguments[0].ToCode(pBuilderContext, pCodeWriter);
            pBuilderContext.BuildingContext.Remove(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY);

            pCodeWriter.AppendTop(" = ");

            pBuilderContext.BuildingContext.Add(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY, "null");
            Arguments[1].ToCode(pBuilderContext, pCodeWriter);
            pBuilderContext.BuildingContext.Remove(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY);

            pCodeWriter.AppendTop(";");
        }

        public CodeCreatorParameter[] GetParameters()
        {
            return new[] {
                new CodeCreatorParameter() {
                 Direction = VariableDirection.Out,
                 Name = "Left",
                 Type = typeof(LeftSideCSharpCode)
                },
                new CodeCreatorParameter() {
                 Direction = VariableDirection.In,
                 Name = "Right",
                 Type = typeof(CSharpCode)
                }
            };
        }
    }
}
