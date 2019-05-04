using System;
using System.Collections.Generic;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;

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

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeWriter.WriteIdentifierTagTop(this);

            Arguments[0].ToCode(pBuilderContext, pCodeWriter, pContainer);
            pCodeWriter.AppendTop(" = ");
            Arguments[1].ToCode(pBuilderContext, pCodeWriter, pContainer);
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
