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

                /*
                   foreach (Type t in a.GetTypes())
                     {
                         if (t.AssemblyQualifiedName == pTypename)
                             return t;
                     } */
            }

            /*
            if (pTypename.Contains("`1[["))
            {
                Type baseType = SearchType(mRegex.Replace(pTypename, ""));
                Type firstType = SearchType(mRegex.Match(pTypename).Groups[1].Value);
                return baseType.MakeGenericType(firstType);
            }

            //No better way for generic types?
            */

            return null;
        }
    }
}
