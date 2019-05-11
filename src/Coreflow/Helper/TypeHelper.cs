using Coreflow.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Helper
{
    public class TypeHelper
    {
        public static Type SearchType(string pTypename)
        {
            var type = Type.GetType(pTypename);
            if (type != null)
                return type;

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(pTypename);
                if (type != null)
                    return type;
            }

            Console.WriteLine($"Warning: Type {pTypename} not found!");
            return null;
        }

        public static string GetDefaultInitializationCodeSnippet(Type pType)
        {
            if (pType == typeof(CSharpCode))
                return string.Empty;

            if (pType.Name == typeof(IEnumerable<>).Name)
            {
                return GetCodeForEmptyIEnumerable(pType);
            }

            return "default(" + pType.FullName + ")";
        }

        public static string GetCodeForEmptyIEnumerable(Type pType)
        {
            if (pType.Name != typeof(IEnumerable<>).Name)
                throw new ArgumentException("Type is not IEnumerable", nameof(pType));

            Type genericType = pType.GenericTypeArguments.Single();

            return $"Enumerable.Empty<{genericType.FullName}>()";
        }

        public static bool IsValidVariableName(string pName)
        {
            return SyntaxFacts.IsValidIdentifier(pName);
        }
    }
}
