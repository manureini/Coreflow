using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Extensions
{
    public static class StringExtensions
    {
        public static string GetIconClassName(this string pIconString)
        {
            if (pIconString == null)
                return "fa " + DisplayMetaAttribute.DEFAULT_ICON;

            if (pIconString.Contains("fa-"))
                return "fa " + pIconString;

            throw new NotSupportedException($"Icon '{pIconString}' is not supported");
        }

    }
}
