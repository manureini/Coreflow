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
        }

        public OutputVariableNameCreator(string pName, string pVariableName, Guid pIdentifier) : this(pName, pVariableName)
        {
            Identifier = pIdentifier;
        }

        public override void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTag(this);
            IVariableCreator existing = WorkflowBuilderHelper.GetParentVariableCreator(pContainer, c => c.VariableIdentifier == VariableIdentifier);
            pCodeWriter.AppendLine($"out {(existing == null ? "var " : " ")}{Code}");
        }
    }
}
