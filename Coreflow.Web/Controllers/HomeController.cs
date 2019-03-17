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
            WorkflowDefinition wfdef = Program.CoreflowInstance.WorkflowDefinitionFactory.Create("new");

            wfdef.Arguments.Add(new WorkflowArguments("test", typeof(string)));

            Program.CoreflowInstance.WorkflowDefinitionStorage.Remove(wfdef.Identifier);
            Program.CoreflowInstance.WorkflowDefinitionStorage.Add(wfdef);

            return RedirectToAction(nameof(Editor), new { id = wfdef.Identifier });
        }

        public IActionResult Editor(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Workflows));

            WorkflowDefinition wfDef = Program.CoreflowInstance.WorkflowDefinitionStorage.GetWorkflowDefinitions().FirstOrDefault(wf => wf.Identifier == id);

            //   WorkflowDefinition wfDef = Program.CoreflowInstance.WorkflowDefinitionFactory.Create("test");

            var workflowDefinitionModel = WorkflowDefinitionModelMappingHelper.GenerateModel(wfDef);

            string serialized = WorkflowDefinitionModelSerializer.Serialize(workflowDefinitionModel);

            HttpContext.Session.SetString("WorkflowModel", serialized);

            return View(workflowDefinitionModel);
        }

        public IActionResult Workflows()
        {
            var workflows = Program.CoreflowInstance.WorkflowDefinitionStorage.GetWorkflowDefinitions();
            return View(workflows);
        }

        public IActionResult DeleteWorkflow(Guid id)
        {
            Program.CoreflowInstance.WorkflowDefinitionStorage.Remove(id);
            return RedirectToAction(nameof(Workflows));
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
