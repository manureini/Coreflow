using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Web.Extensions
{
    public static class CodeCreatorParameterExtensions
    {
        public static List<CodeCreatorParameterModel> ConvertToModel(this CodeCreatorParameter[] pList)
        {
            var ret = new List<CodeCreatorParameterModel>();

            for (int i = 0; i < pList.Count(); i++)
            {
                var cParameter = pList[i];
                ret.Add(new CodeCreatorParameterModel(cParameter.Name, cParameter.DisplayName, cParameter.Type, cParameter.Category, cParameter.Direction, i));
            }

            return ret;
        }

        public static ArgumentModel ConvertToModel(this IArgument pArgument)
        {
            string type = null;

            if (pArgument is InputExpressionCreator iec)
                type = iec.Type;

            return new ArgumentModel(pArgument.Identifier, pArgument.Name, type, pArgument.Code);
        }
    }
}
