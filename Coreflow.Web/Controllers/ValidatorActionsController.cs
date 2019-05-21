using Coreflow.Helper;
using Coreflow.Validation;
using Coreflow.Web.Helper;
using Coreflow.Web.Models;
using Coreflow.Web.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Coreflow.Web.Controllers
{
    [Route("Home/Validator/Action/[action]")]
    [Authorize]
    public class ValidatorActionsController : Controller
    {

        [HttpPost]
        public JsonResult DoValidatorAction([FromBody]IdValueRequest pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                FlowDefinition wfDef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(wfDefModel);

                CorrectorData cData = JsonConvert.DeserializeObject<CorrectorData>(pData.Value);

                Type type = TypeHelper.SearchType(cData.Type);

                //Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)

                type.GetMethod("Correct", BindingFlags.Static | BindingFlags.Public).Invoke(null, new[] { wfDef, cData.CodeCreators, cData.Data });

                wfDefModel = FlowDefinitionModelMappingHelper.GenerateModel(wfDef);

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                return Json(new Response(true, string.Empty));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

    }
}
