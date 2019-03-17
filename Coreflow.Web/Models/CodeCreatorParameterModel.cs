using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Models
{
    public class CodeCreatorParameterModel : CodeCreatorParameter
    {
        public int Index;

        [Obsolete("Serializer")]
        public CodeCreatorParameterModel() : base()
        {
        }

        public CodeCreatorParameterModel(string pName, string pDisplayName, Type pType, string pCategory, ParameterDirection pDirection, int pIndex) : base(pName, pDisplayName, pType, pCategory, pDirection)
        {
            Index = pIndex;
        }
    }
}
