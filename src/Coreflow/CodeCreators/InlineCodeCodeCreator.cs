using System;
using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class InlineCodeCodeCreator : ICodeCreator, IParametrized, IUiDesignable
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public string Name => "Inline Code";

        public string Icon => "fa-code";

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTag(this);
            Arguments[0].ToCode(pBuilderContext, pCodeWriter, pContainer);
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
