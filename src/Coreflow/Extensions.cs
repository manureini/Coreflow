using Coreflow.Interfaces;
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
    }
}
