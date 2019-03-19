using Coreflow.Objects;
using System;

namespace Coreflow.Interfaces
{
    public class InputExpressionCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; } = Guid.NewGuid().ToString();

        public string LocalObjectName { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public InputExpressionCreator()
        {
        }

        public InputExpressionCreator(string pName, string pExpression) : this()
        {
            Name = pName;
            Code = pExpression;
        }

        public InputExpressionCreator(string pName, string pExpression, Guid pIdentifier) : this()
        {
            Name = pName;
            Code = pExpression;
            Identifier = pIdentifier;
        }

        public void Initialize(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodeWriter)
        {
        }

        public void ToCode(WorkflowBuilderContext pBuilderContext, WorkflowCodeWriter pCodewriter, ICodeCreatorContainerCreator pContainer = null)
        {
            pCodewriter.WriteIdentifierTagTop(this);

            if (string.IsNullOrWhiteSpace(Code))
            {
                pCodewriter.AppendLineTop("null");
                return;
            }

            pCodewriter.AppendLineTop(Code);
        }
    }
}
