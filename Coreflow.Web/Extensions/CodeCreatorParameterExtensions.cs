using Coreflow.Objects;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
