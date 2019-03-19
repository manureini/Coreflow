using Coreflow.Objects;
using System;

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
            pCodeWriter.WriteIdentifierTag(this);
            bool existing = false; // WorkflowBuilderHelper.GetVariableCreatorInScope(pContainer, this, c => c.VariableIdentifier == VariableIdentifier);


            pCodeWriter.AppendLine($"out {(existing ? "var " : " ")}{Code}");
        }
    }
}
