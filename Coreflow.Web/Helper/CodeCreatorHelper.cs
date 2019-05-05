using Coreflow.Web.Controllers;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Helper
{
    public static class CodeCreatorHelper
    {
        public static List<CodeCreatorModel> GetCodeCreatorModels()
        {
            return Program.CoreflowInstance.CodeCreatorStorage.GetAllFactories().Select(v =>
             {
                 var model = CodeCreatorModelHelper.CreateModel(v.Create(), null, null);
                 model.CustomFactory = v.Identifier;
                 model.Identifier = Guid.Empty;
                 model.Arguments?.ForEach(v => v.Guid = Guid.Empty);

                 return model;
             }).ToList();
        }
    }
}
