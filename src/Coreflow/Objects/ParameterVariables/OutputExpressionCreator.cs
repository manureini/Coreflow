using Coreflow.Helper;
using Coreflow.Objects;
using System;
using System.Linq;

namespace Coreflow.Interfaces
{
    public class OutputExpressionCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; set; } = Guid.NewGuid().ToString();

        public string Code { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public OutputExpressionCreator() { }

        public OutputExpressionCreator(string pName, Type pType)
        {
            Name = pName;
            Type = pType;
        }

        public virtual void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            if (string.IsNullOrWhiteSpace(Code))
            {
                pCodeWriter.AppendLineTop(TypeHelper.GetDefaultInitializationCodeSnippet(Type, pBuilderContext));
                return;
            }

            bool isSimpleVariableName = !Code.Trim().Contains(" ") && !Code.Contains("\"");

            if (isSimpleVariableName)
            {
                bool existing = pBuilderContext.CurrentSymbols.Any(s => s.Name == Code);

                if (Type == typeof(LeftSideCSharpCode))
                {
                    pCodeWriter.AppendLineTop($"{(!existing ? "var " : " ")}{Code}");
                }
                else
                {
                    pCodeWriter.AppendLineTop($"out {(!existing ? "var " : " ")}{Code}");
                }
            }
            else
            {
                pCodeWriter.AppendLineTop(Code);
            }
        }
    }
}
