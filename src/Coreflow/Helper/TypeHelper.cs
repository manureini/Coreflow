using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Coreflow.Helper
{
    public class TypeHelper
    {
        private static Regex mRegex = new Regex(@"\[\[(.*)\]\]");

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

            return "default(" + pType.Name + ")";
        }

        public static string GetCodeForEmptyIEnumerable(Type pType)
        {
            if (pType.Name != typeof(IEnumerable<>).Name)
                throw new ArgumentException("Type is not IEnumerable", nameof(pType));

            Type genericType = pType.GenericTypeArguments.Single();

            return $"Enumerable.Empty<{genericType.FullName}>()";
        }
    }
}
