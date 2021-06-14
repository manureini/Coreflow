﻿using Coreflow.Helper;
using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Coreflow.Interfaces
{
    public class InputExpressionCreator : IVariableCreator, IArgument
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier { get; set; } = Guid.NewGuid().ToString();

        public string Code { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public string DefaultValueCode { get; set; }

        public ITypeSymbol ActualType { get; internal set; }

        public InputExpressionCreator()
        {
        }

        public InputExpressionCreator(string pName, string pExpression) : this()
        {
            Name = pName;
            Code = pExpression;
        }

        public InputExpressionCreator(string pName, string pExpression, Guid pIdentifier, Type pType, string pDefaultValueCode) : this(pName, pExpression)
        {
            Identifier = pIdentifier;
            Type = pType;
            DefaultValueCode = pDefaultValueCode;
        }

        public InputExpressionCreator(string pName, Type pType, string pDefaultValueCode)
        {
            Name = pName;
            Type = pType;
            DefaultValueCode = pDefaultValueCode;
        }

        public void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodewriter)
        {
            pCodewriter.WriteIdentifierTagTop(this);

            Code = Code?.Trim();

            if (string.IsNullOrWhiteSpace(Code))
            {
                if (!string.IsNullOrWhiteSpace(DefaultValueCode))
                {
                    if (!(TypeHelper.IsValidVariableName(DefaultValueCode) && !pBuilderContext.CurrentSymbols.Any(s => s.Name == DefaultValueCode)))
                    {
                        if (Type.IsByRef && !DefaultValueCode.StartsWith("ref "))
                            DefaultValueCode = "ref " + DefaultValueCode;
                        pCodewriter.AppendLineTop(DefaultValueCode);
                        return;
                    }
                }

                pCodewriter.AppendLineTop(TypeHelper.GetDefaultInitializationCodeSnippet(Type, pBuilderContext));
                return;
            }

            if (Code.StartsWith("$"))
            {
                string name = Code.Substring(1);

                if (TypeHelper.IsValidVariableName(name))
                {
                    pCodewriter.AppendLineTop($"({Type.FullName})CoreflowInstace.ArgumentInjectionStore.GetArgumentValue(\"{name}\", typeof({Type.FullName}))");
                    return;
                }
            }

            try
            {
                /*
                if (ActualType != null && type != typeof(CSharpCode) && !ActualType.TypeSymbolMatchesType(type, pBuilderContext.SemanticModel))
                {
                    ITypeSymbol typeSymbol = type.GetTypeSymbolForType(pBuilderContext.SemanticModel);

                    if (typeSymbol != null) //does not work on linux
                    {
                        var converation = pBuilderContext.Compilation.ClassifyCommonConversion(ActualType, typeSymbol);

                        if (!(converation.IsIdentity | converation.IsImplicit | converation.IsNumeric | converation.IsUserDefined))
                        {
                            if (ParameterConverterHelper.AppendCodeWithTypeConverter(pCodewriter, ActualType, typeSymbol, Code))
                                return; // found a conversation
                        }
                    }
                }*/
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
