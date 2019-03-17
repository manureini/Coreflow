using Coreflow.Objects;
using System;

namespace Coreflow.Interfaces
{
    public class OutputVariableCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; } = Guid.NewGuid().ToString();

        //Because this is Output Code = VariableName
        public string Code { get; set; }

        public string Name { get; set; }

        public OutputVariableCreator()
        {
        }

        public OutputVariableCreator(string pName, string pVariableName) : this()
        {
            Code = pVariableName;
        }

        public OutputVariableCreator(string pName, string pVariableName, Guid pIdentifier) : this()
        {
            Code = pVariableName;
            Identifier = pIdentifier;
        }

        public void Initialize(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter)
        {
        }

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTag(this);

            IVariableCreator existing = WorkflowBuilderHelper.GetParentVariableCreator(pContainer, c => c.VariableIdentifier == VariableIdentifier);
            pCodeWriter.AppendLine($"out {(existing == null ? "var " : " ")}{Code}");
        }
    }
}
