using Coreflow.Objects;
using System;

namespace Coreflow.Interfaces
{
    public class OutputVariableCodeInlineCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; set; } = Guid.NewGuid().ToString();

        public string Code { get; set; }

        public string Name { get; set; }

        public OutputVariableCodeInlineCreator()
        {
        }

        public OutputVariableCodeInlineCreator(string pName, string pCode) : this()
        {
            Name = pName;
            Code = pCode;
        }

        public OutputVariableCodeInlineCreator(string pName, string pCode, Guid pIdentifier) : this(pName, pCode)
        {
            Identifier = pIdentifier;
        }

        public void Initialize(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter)
        {
        }

        public virtual void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTag(this);
            pCodeWriter.AppendLine(Code);
        }
    }

}
