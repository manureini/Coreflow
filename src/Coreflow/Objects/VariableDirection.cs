using System;

namespace Coreflow.Objects
{
    public enum VariableDirection
    {
        In,
        Out,
        InOut
    }

    public static class VariableDirectionHelper
    {

        public static VariableDirection Parse(string pValue)
        {

#if NETCOREAPP
            return Enum.Parse<VariableDirection>(pValue);
#else
            return (VariableDirection)Enum.Parse(typeof(VariableDirection), pValue);
#endif

        }



    }

}
