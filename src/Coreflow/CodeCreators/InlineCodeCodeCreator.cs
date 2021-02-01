using System;
using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class InlineCodeCodeCreator : ICodeCreator, IParametrized, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public string Name => "Inline Code";

        public string Icon => "fa-code";

        public string Category => "Basic";

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            Arguments[0].ToCode(pBuilderContext, pCodeWriter);
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
