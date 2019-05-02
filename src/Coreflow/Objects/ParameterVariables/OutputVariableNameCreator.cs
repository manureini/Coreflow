using Coreflow.Objects;
using System;
using System.Linq;

namespace Coreflow.Interfaces
{
    public class OutputVariableNameCreator : AbstractVariableCreator
    {
        public OutputVariableNameCreator()
        {
        }

        public OutputVariableNameCreator(string pName, string pVariableName) : base(pName, pVariableName, pName)
        {
        }

        public OutputVariableNameCreator(string pName, string pVariableName, Guid pIdentifier) : base(pName, pVariableName, pName, pIdentifier)
        {
        }

        public override void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            if (string.IsNullOrWhiteSpace(Code))
            {
                Code = "null";
            }

            bool existing = pBuilderContext.CurrentSymbols.Any(s => s.Name == Code);

            pCodeWriter.AppendLineTop($"out {(!existing ? "var " : " ")}{Code}");
        }
    }
}
