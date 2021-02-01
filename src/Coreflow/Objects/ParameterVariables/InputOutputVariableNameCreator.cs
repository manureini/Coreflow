using Coreflow.Objects;
using System;
using System.Linq;

namespace Coreflow.Interfaces
{
    public class InputOutputVariableNameCreator : AbstractVariableCreator
    {
        public InputOutputVariableNameCreator()
        {
        }

        public InputOutputVariableNameCreator(string pName) : base(pName)
        {
        }

        public InputOutputVariableNameCreator(string pName, string pVariableName) : base(pName, pVariableName, pName)
        {
        }

        public InputOutputVariableNameCreator(string pName, string pVariableName, Guid pIdentifier) : base(pName, pVariableName, pName, pIdentifier)
        {
        }

        public override void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            if (string.IsNullOrWhiteSpace(Code))
            {
                Code = "null";
            }

            bool existing = pBuilderContext.CurrentSymbols.Any(s => s.Name == Code);

            pCodeWriter.AppendLineTop($"ref {(!existing ? "var " : " ")}{Code}");
        }
    }
}
