using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Coreflow.Helper
{
    public static class ParameterConverterHelper
    {
        public static bool AppendCodeWithTypeConverter(FlowCodeWriter pCodeWriter, ITypeSymbol pFrom, ITypeSymbol pTo, string pCode)
        {
            if (pTo.MetadataName == typeof(IEnumerable<>).Name && pFrom.MetadataName != typeof(IEnumerable<>).Name)
            {
                pCodeWriter.AppendLineTop("new [] { " + pCode + " }");
                return true;
            }

            return false;
        }




    }
}
