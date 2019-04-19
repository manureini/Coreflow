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

        //Type "Type" and serializer does not work
        public string Type { get; set; }

        public string FactoryIdentifier { get; set; }

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

                if (type == typeof(CSharpCode))
                    return;

                pCodewriter.AppendLineTop("default(" + type.Name + ")");
                return;
            }

            pCodewriter.AppendLineTop(Code);
        }
    }
}
