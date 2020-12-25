using Coreflow.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow
{
    public static class Extensions
    {
        private const string VARIABLE_PREFIX = "__cc_";

        public static string ToVariableName(this string pIdentifier)
        {
            char[] notAllowed = new char[]
            {
                '-', '@', '"', '\''
            };

            return VARIABLE_PREFIX + notAllowed.Aggregate(pIdentifier, (c1, c2) => c1.Replace(c2, '_'));
        }

        public static bool IsContainerCreatorVariableName(this string pString)
        {
            return pString.StartsWith(VARIABLE_PREFIX);
        }

        public static void Set(this IDictionary<string, IArgument> pDictionary, string pKey, IArgument pValue)
        {
            pDictionary.Remove(pKey);
            pDictionary.Add(pKey, pValue);
        }

        public static bool TypeSymbolMatchesType(this ITypeSymbol typeSymbol, Type type, SemanticModel semanticModel)
        {
            return typeSymbol == GetTypeSymbolForType(type, semanticModel);
        }

        public static INamedTypeSymbol GetTypeSymbolForType(this Type type, SemanticModel semanticModel)
        {
            if (!type.IsConstructedGenericType)            
                return semanticModel.Compilation.GetTypeByMetadataName(type.FullName);
            
            // get all typeInfo's for the Type arguments 
            var typeArgumentsTypeInfos = type.GenericTypeArguments.Select(a => GetTypeSymbolForType(a, semanticModel));

            var openType = type.GetGenericTypeDefinition();
            var typeSymbol = semanticModel.Compilation.GetTypeByMetadataName(openType.FullName);

            if (typeSymbol == null)
                return null;

            return typeSymbol.Construct(typeArgumentsTypeInfos.ToArray<ITypeSymbol>());
        }

        public static string ToTypeName(this ITypeSymbol pTypeSymbol)
        {
            var symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
            return pTypeSymbol.ToDisplayString(symbolDisplayFormat);
        }

        public static string GetTypeIdentifier(this ICodeCreator pCodeCreator)
        {
            if (pCodeCreator is ICustomFactoryCodeCreator c)
            {
                return c.FactoryIdentifier;
            }

            return pCodeCreator.GetType().FullName;
        }
    }
}
