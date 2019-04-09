using Coreflow.Objects;
using System;
using System.Linq;

namespace Coreflow.Interfaces
{
    public class LeftSideVariableNameCreator : OutputVariableCodeInlineCreator
    {
        public LeftSideVariableNameCreator()
        {
        }

        public LeftSideVariableNameCreator(string pName, string pVariableName) : this()
        {
            Name = pName;
            Code = pVariableName;
            VariableIdentifier = pName;
        }

        public LeftSideVariableNameCreator(string pName, string pVariableName, Guid pIdentifier) : this(pName, pVariableName)
        {
            Identifier = pIdentifier;
        }

        public override void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            if (string.IsNullOrWhiteSpace(Code))
            {
                Code = "null";
            }

            bool existing = pBuilderContext.CurrentSymbols.Any(s => s.Name == Code);

            pCodeWriter.AppendLineTop($"{(!existing ? "var " : " ")}{Code}");
        }
    }
}
