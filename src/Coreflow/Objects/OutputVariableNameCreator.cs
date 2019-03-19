using Coreflow.Objects;
using System;
using System.Linq;

namespace Coreflow.Interfaces
{
    public class OutputVariableNameCreator : OutputVariableCodeInlineCreator
    {
        public OutputVariableNameCreator()
        {
        }

        public OutputVariableNameCreator(string pName, string pVariableName) : this()
        {
            Name = pName;
            Code = pVariableName;
            VariableIdentifier = pName;
        }

        public OutputVariableNameCreator(string pName, string pVariableName, Guid pIdentifier) : this(pName, pVariableName)
        {
            Identifier = pIdentifier;
        }

        public override void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            // WorkflowBuilderHelper.GetVariableCreatorInScope(pContainer, this, c => c.VariableIdentifier == VariableIdentifier);
                       
            bool existing = pBuilderContext.CurrentSymbols.Any(s => s.Name == Code);

            pCodeWriter.AppendLineTop($"out {(!existing ? "var " : " ")}{Code}");
        }
    }
}
