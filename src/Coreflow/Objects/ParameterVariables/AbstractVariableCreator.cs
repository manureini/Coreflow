using Coreflow.Objects;
using System;

namespace Coreflow.Interfaces
{
    public abstract class AbstractVariableCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; set; } = Guid.NewGuid().ToString();

        public string Code { get; set; }

        public string Name { get; set; }

        public AbstractVariableCreator()
        {
        }

        public AbstractVariableCreator(string pName) : this()
        {
            Name = pName;
        }

        public AbstractVariableCreator(string pName, string pCode, string pVariableIdentifier) : this()
        {
            Name = pName;
            Code = pCode;
            VariableIdentifier = pVariableIdentifier;
        }

        public AbstractVariableCreator(string pName, string pCode, string pVariableIdentifier, Guid pIdentifier) : this(pName, pCode, pVariableIdentifier)
        {
            Identifier = pIdentifier;
        }

        public virtual void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
        }

        public abstract void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer = null);
    }
}
