using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Coreflow.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Coreflow.Web.Helper;
using Coreflow.Objects.CodeCreatorFactory;
using Coreflow.Validation;
using System.IO;
using System.Linq;
using Coreflow.Helper;
using System.Text;
using System.Net.Mime;

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

            var fmodel = FlowDefinitionModelMappingHelper.GenerateModel(wfdef);

            FlowDefinitionModelStorage.StoreModel(fmodel, false);

            return RedirectToAction(nameof(Editor), new { id = wfdef.Identifier });
        }


        public IActionResult Upload()
        {
            var file = Request.Form.Files.FirstOrDefault();

            using (Stream readStream = file.OpenReadStream())
            using (MemoryStream ms = new MemoryStream())
            {
                readStream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);

                string serialized = Encoding.UTF8.GetString(ms.GetBuffer());

                var fdef = FlowDefinitionSerializer.Deserialize(serialized, Program.CoreflowInstance);
                Program.CoreflowInstance.FlowDefinitionStorage.Add(fdef);

                return Json(new Response(true, "ok"));

            }
        }

        public IActionResult Editor(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Flows));

            var fmodel = FlowDefinitionModelStorage.GetModel(id);

            var fdef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(fmodel);
            var result = FlowValidationHelper.Validate(fdef);

            if (result.Messages.Count > 0)
            {
                return RedirectToAction(nameof(Validator), new { id = fmodel.Identifier });
            }

            return View(fmodel);
        }

        public IActionResult Validator(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Flows));

            var fmodel = FlowDefinitionModelStorage.GetModel(id);

            var fdef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(fmodel);

            var result = FlowValidationHelper.Validate(fdef);

            if (result.Messages.Count <= 0)
            {
                return RedirectToAction(nameof(Editor), new { id = fmodel.Identifier });
            }

            var corrector = CorrectorHelper.GetCorrectors(fdef, result.Messages);

            return View(new ValidatorModel()
            {
                Correctors = corrector,
                FlowDefinition = fmodel,
                ValidationResult = result
            });
        }

        public IActionResult Flows()
        {
            var Flows = Program.CoreflowInstance.FlowDefinitionStorage.GetDefinitions();
            return View(Flows);
        }

        public IActionResult Instances()
        {
            var Flows = Program.CoreflowInstance.FlowInstanceStorage.GetInstances();
            return View(Flows);
        }

        public IActionResult DeleteFlow(Guid id)
        {
            Program.CoreflowInstance.FlowDefinitionStorage.Remove(id);

            FlowDefinitionModelStorage.ResetFlow(id);

            string factoryIdentifier = CallFlowCreatorFactory.GetIdentifier(id);
            Program.CoreflowInstance.CodeCreatorStorage.RemoveFactory(factoryIdentifier);

            return RedirectToAction(nameof(Flows));
        }

        [HttpPost]
        public FileResult DownloadBinary()
        {
            Program.CoreflowInstance.CompileFlows();
            if (Program.CoreflowInstance.LastCompileResult == null)
            {
                return null;
            }

            string filepath = Program.CoreflowInstance.LastCompileResult.DllFilePath;
            var fs = System.IO.File.OpenRead(filepath);
            return File(fs, MediaTypeNames.Application.Octet, "Flows.dll");
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
