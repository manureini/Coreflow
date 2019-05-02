using Coreflow.Objects;
using System;

namespace Coreflow.Interfaces
{
    public class OutputVariableCodeInlineCreator : IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

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

        public virtual void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            pCodeWriter.AppendLineTop(Code);
        }
    }
}
