using Coreflow.Helper;
using Coreflow.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coreflow.Web.Controllers
{
    [Route("Home/Validator/Action/[action]")]
    [Authorize]
    public class ValidatorActionsController : Controller
    {


        [HttpPost]
        public JsonResult DoValidatorAction([FromBody]CorrectorData pData)
        {

            Type type = TypeHelper.SearchType(pData.Type);


            //Correct(FlowDefinition pFlowDefinition, List<Guid> pCodeCreators, object pData)

           // type.GetMethod("Correct").Invoke(null, new[] { });


            return new JsonResult("");
        }




    }
}
