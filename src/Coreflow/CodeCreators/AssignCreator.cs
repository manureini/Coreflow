using System;
using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class AssignCreator : ICodeCreator, IParametrized, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public string Name => "Assign";

        public string Icon => "fa-equals";

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
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
                 Type = typeof(CSharpCode)
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
