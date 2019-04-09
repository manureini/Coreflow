using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Coreflow.Web.Models;
using Microsoft.AspNetCore.Http;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Web.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Coreflow.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            FlowDefinition wfdef = Program.CoreflowInstance.FlowDefinitionFactory.Create("new");

            wfdef.Arguments.Add(new FlowArguments("test", typeof(string), VariableDirection.In, ""));
            wfdef.Arguments.Add(new FlowArguments("result", typeof(string), VariableDirection.Out, ""));

            Program.CoreflowInstance.FlowDefinitionStorage.Remove(wfdef.Identifier);
            Program.CoreflowInstance.FlowDefinitionStorage.Add(wfdef);

            return RedirectToAction(nameof(Editor), new { id = wfdef.Identifier });
        }

        public IActionResult Editor(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Flows));

            FlowDefinition wfDef = Program.CoreflowInstance.FlowDefinitionStorage.GetFlowDefinitions().FirstOrDefault(wf => wf.Identifier == id);

            //   FlowDefinition wfDef = Program.CoreflowInstance.FlowDefinitionFactory.Create("test");

            var FlowDefinitionModel = FlowDefinitionModelMappingHelper.GenerateModel(wfDef);

            string serialized = FlowDefinitionModelSerializer.Serialize(FlowDefinitionModel);

            HttpContext.Session.SetString("FlowModel", serialized);

            return View(FlowDefinitionModel);
        }

        public IActionResult Flows()
        {
            var Flows = Program.CoreflowInstance.FlowDefinitionStorage.GetFlowDefinitions();
            return View(Flows);
        }

        public IActionResult DeleteFlow(Guid id)
        {
            Program.CoreflowInstance.FlowDefinitionStorage.Remove(id);
            return RedirectToAction(nameof(Flows));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Monaco()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
