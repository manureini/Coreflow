using Coreflow.Helper;
using Coreflow.Objects;
using System;

namespace Coreflow.Interfaces
{
    public class InputExpressionCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; internal set; } = Guid.NewGuid().ToString();

        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public InputExpressionCreator()
        {
        }

        public InputExpressionCreator(string pName, string pExpression) : this()
        {
            Name = pName;
            Code = pExpression;
        }

        public InputExpressionCreator(string pName, string pExpression, Guid pIdentifier, Type pType) : this()
        {
            Name = pName;
            Code = pExpression;
            Identifier = pIdentifier;
            Type = pType.AssemblyQualifiedName;
        }

        public void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodewriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodewriter.WriteIdentifierTagTop(this);

            if (string.IsNullOrWhiteSpace(Code))
            {
                Type type = TypeHelper.SearchType(Type);
                pCodewriter.AppendLineTop("default(" + type.Name + ")");
                return;
            }

            pCodewriter.AppendLineTop(Code);
        }
    }
}
