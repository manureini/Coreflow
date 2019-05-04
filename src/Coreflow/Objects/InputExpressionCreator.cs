using Coreflow.Helper;
using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;

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

        public ITypeSymbol ActualType { get; internal set; }

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

            Type type = TypeHelper.SearchType(Type);

            Code = Code.Trim();


            //    var coreflowInstance = pBuilderContext.FlowDefinition.Coreflow;

            if (Code.StartsWith("$"))
            {
                string name = Code.Substring(1);

                if (TypeHelper.IsValidVariableName(name))
                {


                }

                // coreflowInstance.ArgumentInjectionStore.GetArgumentValue(name, type);
            }


            if (string.IsNullOrWhiteSpace(Code))
            {
                pCodewriter.AppendLineTop(TypeHelper.GetDefaultInitializationCodeSnippet(type));
                return;
            }

            try
            {
                if (ActualType != null && type != typeof(CSharpCode) && !ActualType.TypeSymbolMatchesType(type, pBuilderContext.SemanticModel))
                {
                    ITypeSymbol typeSymbol = type.GetTypeSymbolForType(pBuilderContext.SemanticModel);

                    var converation = pBuilderContext.Compilation.ClassifyCommonConversion(ActualType, typeSymbol);

                    if (!(converation.IsIdentity | converation.IsImplicit | converation.IsNumeric | converation.IsUserDefined))
                    {
                        if (ParameterConverterHelper.AppendCodeWithTypeConverter(pCodewriter, ActualType, typeSymbol, Code))
                            return; // found a conversation
                    }
                }
            }
            catch (Exception e)
            {
                //TODO  does not work with linux
                Console.WriteLine(e);
            }

            pCodewriter.AppendLineTop(Code);
        }
    }
}
