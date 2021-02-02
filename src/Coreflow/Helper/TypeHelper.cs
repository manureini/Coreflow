using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.ParameterVariables;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coreflow.Helper
{
    public static class TypeHelper
    {
        static TypeHelper()
        {
            SearchType(typeof(IDictionary<,>).FullName);
            SearchType(typeof(List<>).FullName);
            SearchType(typeof(IEnumerable<>).FullName);
        }

        private static Regex mGenericTypeCSharpRegex = new Regex(@"(.*)`([0-9]*)\[(?:\[(.+?)\]?,?)*\]");
        private static Regex mGenericTypeCodeRegex = new Regex(@"(.*?)<(?:(.*?)[,|>])*");

        public static Type SearchType(string pTypeName)
        {
            if (pTypeName == null)
                return null;

            pTypeName = pTypeName.Trim();

            try
            {
                var type = Type.GetType((string)pTypeName);
                if (type != null)
                    return type;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = asm.GetTypes().FirstOrDefault(t => t.FullName == pTypeName);
                /*
                var type = asm.GetType(pTypeName); */

                if (type != null)
                    return type;
            }

            var result = ParseAndFindSimpleType(pTypeName);
            if (result != null)
                return result;

            if (pTypeName.Contains("<"))
            {
                var match = mGenericTypeCodeRegex.Match(pTypeName);

                if (match.Success)
                {
                    string baseTypeName = match.Groups[1].Value;
                    var genericParamCount = match.Groups[2].Captures.Count;

                    var genericTypes = new Type[genericParamCount];

                    for (int i = 0; i < genericParamCount; i++)
                    {
                        genericTypes[i] = SearchType(match.Groups[2].Captures[i].Value);
                    }

                    var baseType = SearchType(baseTypeName + "`" + genericParamCount);

                    return baseType.MakeGenericType(genericTypes);
                }
            }

            if (pTypeName.Contains("`"))
            {
                var match = mGenericTypeCSharpRegex.Match(pTypeName);

                if (match.Success)
                {
                    string baseTypeName = match.Groups[1].Value;

                    int genericParamCount = Convert.ToInt32(match.Groups[2].Value);

                    if (genericParamCount != match.Groups[3].Captures.Count)
                        return null;

                    var genericTypes = new Type[genericParamCount];

                    for (int i = 0; i < genericParamCount; i++)
                    {
                        genericTypes[i] = SearchType(match.Groups[3].Captures[i].Value);
                    }

                    var baseType = SearchType(baseTypeName + "`" + genericParamCount);

                    return baseType.MakeGenericType(genericTypes);
                }
            }

            Console.WriteLine($"Warning: Type {pTypeName} not found!");
            return null;
        }

        private static Type ParseAndFindSimpleType(string pTypeName)
        {
            bool isArray = false;
            bool isNullable = false;

            if (pTypeName.Contains("[]") && !pTypeName.Contains("`"))
            {
                isArray = true;
                pTypeName = pTypeName.Remove(pTypeName.IndexOf("[]"), 2);
            }

            if (pTypeName.EndsWith("?"))
            {
                isNullable = true;
                pTypeName = pTypeName.Remove(pTypeName.IndexOf("?"), 1);
            }

            pTypeName = pTypeName.ToLower();

            string parsedTypeName = null;
            switch (pTypeName)
            {
                case "bool":
                case "boolean":
                    parsedTypeName = "System.Boolean";
                    break;
                case "byte":
                    parsedTypeName = "System.Byte";
                    break;
                case "char":
                    parsedTypeName = "System.Char";
                    break;
                case "datetime":
                    parsedTypeName = "System.DateTime";
                    break;
                case "datetimeoffset":
                    parsedTypeName = "System.DateTimeOffset";
                    break;
                case "decimal":
                    parsedTypeName = "System.Decimal";
                    break;
                case "double":
                    parsedTypeName = "System.Double";
                    break;
                case "float":
                    parsedTypeName = "System.Single";
                    break;
                case "int16":
                case "short":
                    parsedTypeName = "System.Int16";
                    break;
                case "int32":
                case "int":
                    parsedTypeName = "System.Int32";
                    break;
                case "int64":
                case "long":
                    parsedTypeName = "System.Int64";
                    break;
                case "object":
                    parsedTypeName = "System.Object";
                    break;
                case "sbyte":
                    parsedTypeName = "System.SByte";
                    break;
                case "string":
                    parsedTypeName = "System.String";
                    break;
                case "timespan":
                    parsedTypeName = "System.TimeSpan";
                    break;
                case "uint16":
                case "ushort":
                    parsedTypeName = "System.UInt16";
                    break;
                case "uint32":
                case "uint":
                    parsedTypeName = "System.UInt32";
                    break;
                case "uint64":
                case "ulong":
                    parsedTypeName = "System.UInt64";
                    break;

                case "list`1":
                    parsedTypeName = typeof(List<>).FullName;
                    break;

                case "ilist`1":
                    parsedTypeName = typeof(IList<>).FullName;
                    break;

                case "dictionary`2":
                    parsedTypeName = typeof(Dictionary<,>).FullName;
                    break;

                case "idictionary`2":
                    parsedTypeName = typeof(IDictionary<,>).FullName;
                    break;

                case "ienumerable`1":
                    parsedTypeName = typeof(IEnumerable<>).FullName;
                    break;

                case "ienumerable":
                    parsedTypeName = typeof(IEnumerable).FullName;
                    break;
            }

            if (parsedTypeName == null)
                return null;

            if (isArray)
                parsedTypeName += "[]";

            if (isNullable)
                parsedTypeName = string.Concat("System.Nullable`1[", parsedTypeName, "]");

            return SearchType(parsedTypeName);
        }

        public static string GetDefaultInitializationCodeSnippet(Type pType, FlowBuilderContext pBuilderContext)
        {
            if(pBuilderContext.BuildingContext.ContainsKey(ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY))
            {
                var value = pBuilderContext.BuildingContext[ExpressionCreatorHintsKeys.CUSTOM_DEFAULT_EXPRESSION_EXPRESSION_CREATOR_KEY];

                if (value is string str)
                    return str;

                if (value is Func<Type, string> func)
                    return func(pType);
            }

            if (pType == typeof(CSharpCode))
                return string.Empty;

            if (pType.Name == typeof(IEnumerable<>).Name)
            {
                return GetCodeForEmptyIEnumerable(pType);
            }

            return "default(global::" + pType.FullName + ")";
        }

        public static string GetCodeForEmptyIEnumerable(Type pType)
        {
            if (pType.Name != typeof(IEnumerable<>).Name)
                throw new ArgumentException("Type is not IEnumerable", nameof(pType));

            Type genericType = pType.GenericTypeArguments.Single();

            return $"Enumerable.Empty<global::{genericType.FullName}>()";
        }

        public static bool IsValidVariableName(string pName)
        {
            return SyntaxFacts.IsValidIdentifier(pName);
        }

        public static string TypeNameToCode(Type pType)
        {
            if (pType.IsGenericType && pType.GenericTypeArguments.Length > 0)
            {
                var genTypeDef = TypeNameToCode(pType.GetGenericTypeDefinition());
                return genTypeDef + "<" + string.Join(", ", pType.GetGenericArguments().Select(t => TypeNameToCode(t))) + ">";
            }
            else if (pType.IsGenericType)
            {
                return "global::" + pType.FullName.Substring(0, pType.FullName.IndexOf('`'));
            }

            return "global::" + pType.FullName;
        }
    }
}
