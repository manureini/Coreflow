using Coreflow.Interfaces;
using Coreflow.Web.Controllers;
using Coreflow.Web.Extensions;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Web.Helper
{
    public static class CodeCreatorHelper
    {
        public static List<CodeCreatorModel> GetCodeCreatorModels()
        {
            return Program.CoreflowInstance.CodeCreatorStorage.GetAllFactories().Select(v =>
             {
                 var cc = v.Create();
                 var model = CodeCreatorModelHelper.CreateModel(cc, null, null);
                 model.CustomFactory = v.Identifier;
                 model.Identifier = Guid.Empty;
                 model.Arguments = model.Parameters?.Select(p => new ArgumentModel(Guid.Empty, p.Name, p.Type.AssemblyQualifiedName, "")).ToList() ?? new List<ArgumentModel>();
                 return model;
             }).ToList();
        }
    }
}
