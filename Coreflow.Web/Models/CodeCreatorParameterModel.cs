using Coreflow.Objects;
using System;

namespace Coreflow.Web.Models
{
    public class CodeCreatorParameterModel : CodeCreatorParameter
    {
        public int Index;

        [Obsolete("Serializer")]
        public CodeCreatorParameterModel() : base()
        {
        }

        public CodeCreatorParameterModel(string pName, string pDisplayName, Type pType, string pCategory, VariableDirection pDirection, int pIndex) : base(pName, pDisplayName, pType, pCategory, pDirection)
        {
            Index = pIndex;
        }
    }
}
