using Coreflow.CodeCreators;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.CodeCreatorFactory;
using Coreflow.Web.Extensions;
using Coreflow.Web.Helper;
using Coreflow.Web.Models;
using Coreflow.Web.Models.Requests;
using Coreflow.Web.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coreflow.Web.Controllers
{
    [Route("Home/Editor/Action/[action]")]
    [Authorize]
    public class EditorActionsController : Controller
    {
        [HttpGet]
        public JsonResult GetCodeCreatorDisplayNames()
        {
            var ccs = Program.CoreflowInstance.CodeCreatorStorage.GetAllFactories().Select(f => (f.Identifier, f.Create()));
            return Json(new DictionaryResponse(true, "", ccs.ToDictionary(c => c.Identifier, c => c.Item2.GetDisplayName())));
        }


        [HttpPost]
        public JsonResult SaveFlow([FromBody] FlowEditorRequest pRequest)
        {
            FlowDefinitionModelStorage.PersistModel(pRequest.FlowIdentifier);
            return Json(new Response(true, "ok"));
        }

        [HttpPost]
        public JsonResult ResetFlow([FromBody] FlowEditorRequest pRequest)
        {
            FlowDefinitionModelStorage.ResetFlow(pRequest.FlowIdentifier);
            return Json(new Response(true, "ok"));
        }

        [HttpPost]
        public JsonResult RunFlow([FromBody] FlowEditorRequest pRequest)
        {
            /*  string serialized = HttpContext.Session.GetString("FlowModel");
              FlowDefinitionModel wfDefModel = FlowDefinitionModelSerializer.DeSerialize(serialized);

              FlowDefinition wfDef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(wfDefModel);
              wfDef.Coreflow = Program.CoreflowInstance;

              //TODO
              Program.CoreflowInstance.FlowDefinitionStorage.Remove(wfDef.Identifier);
              Program.CoreflowInstance.FlowDefinitionStorage.Add(wfDef);

              Program.CoreflowInstance.CompileFlows();
              Program.CoreflowInstance.RunFlow(wfDef.Identifier);*/


            Guid flowidentifier = pRequest.FlowIdentifier;

            FlowDefinitionModelStorage.PersistModel(flowidentifier);

            Program.CoreflowInstance.CompileFlows();
            Program.CoreflowInstance.RunFlow(flowidentifier);


            /*

          // FlowInvokeResult invokeResult = FlowInvoker.Invoke(wfDef);

          var factory = wfDef.GenerateFlowCode().Compile().InstanceFactory;

            var resultArgs = factory.RunInstance(new Dictionary<string, object>()
                {
                    { "a", 1 },
                    {"b", 2 }
                });

    */
            string result = "";
            //  string result = invokeResult.ExecutedInstance.GetType().GetField("result").GetValue(invokeResult.ExecutedInstance) as string;

            return Json(new Response(false, result));
        }

        [HttpPost]
        public JsonResult ParameterTextChanged([FromBody] ParameterChangedData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                CodeCreatorModel ccmodel = FlowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, Guid.Parse(pData.CreatorGuid)) as CodeCreatorModel;

                if (ccmodel.Arguments == null)
                    ccmodel.Arguments = new List<ArgumentModel>();

                Guid parameterGuid = Guid.Parse(pData.ParameterGuid);

                ArgumentModel am = ccmodel.Arguments.FirstOrDefault(a => a.Guid == parameterGuid);

                if (am == null)
                    throw new Exception("No Argument with Guid " + parameterGuid + " found!");

                am.Code = pData.NewValue;


                //       FlowDefinition wfDef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(wfDefModel);
                //       FlowCode code = wfDef.GenerateFlowCode();
                //       FlowCompileResult result = code.Compile(false);

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                /*           IDictionary<string, string> errors = new Dictionary<string, string>();

                           if (!result.Successful)
                               errors = result.ErrorCodeCreators.ToDictionary(s => s.Key.ToString(), s => s.Value);
                               */

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        [HttpPost]
        public JsonResult CompileFlow([FromBody] FlowEditorRequest pData)
        {
            /*
             FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

            FlowDefinition wfDef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(wfDefModel);
            FlowCode code = wfDef.GenerateFlowCode();
            FlowCompileResult result = code.Compile(false);
            */

            var combinedCode = new StringBuilder();

            foreach (var flow in FlowDefinitionModelStorage.CombineStoredAndTmpFlows())
            {
                FlowCode fcode = flow.GenerateFlowCode();
                combinedCode.Append(fcode.Code);
            }

            string fullcode = combinedCode.ToString();

            var result = FlowCompilerHelper.CompileFlowCode(fullcode, false, "dummy");


            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (!result.Successful)
                errors = result.ErrorCodeCreators.ToDictionary(s => s.Key.ToString(), s => s.Value);

            return Json(new DictionaryResponse(true, "ok", errors));
        }

        [HttpPost]
        public JsonResult UserDisplayNameChanged([FromBody] UserDisplayNameChangedData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                if (pData.CreatorGuid == "flow-name")
                {
                    wfDefModel.Name = pData.NewValue;
                }
                else
                {
                    CodeCreatorModel ccmodel = FlowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, Guid.Parse(pData.CreatorGuid)) as CodeCreatorModel;
                    ccmodel.UserDisplayName = pData.NewValue;
                }

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);
                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        [HttpPost]
        public JsonResult FlowReferencedNamespaceChanged([FromBody] FlowReferencedNamespaceChangedData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                if (pData.AddValue)
                {
                    wfDefModel.ReferencedNamespaces.Add(pData.Value);
                }
                else
                {
                    wfDefModel.ReferencedNamespaces.Remove(pData.Value);
                }

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }


        [HttpPost]
        public JsonResult FlowArgumentChanged([FromBody] FlowArgumentChangedData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                if (pData.AddValue)
                {
                    Type type = Type.GetType(pData.Type);
                    wfDefModel.Arguments.Add(new FlowArguments(pData.Name, type, VariableDirection.In, pData.Value));
                }
                else
                {
                    wfDefModel.Arguments.RemoveAll(a => a.Name == pData.Name);
                }

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }


        [HttpPost]
        public JsonResult CodeCreatorMoved([FromBody] MoveAfterData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                Guid source = Guid.Parse(pData.SourceId);
                Guid destContainer = Guid.Parse(pData.DestinationContainerId);
                Guid? destAfter = null;

                int? sequenceIndex = null;
                if (int.TryParse(pData.SequenceIndex, out int sindex))
                    sequenceIndex = sindex;

                if (pData.DestinationAfterId != null)
                    destAfter = Guid.Parse(pData.DestinationAfterId);

                CodeCreatorModel sourceModel = FlowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, source) as CodeCreatorModel;

                UpdateCodeCreatorModel(wfDefModel, destAfter, destContainer, sequenceIndex, sourceModel);

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        [HttpPost]
        public JsonResult CodeCreatorDeleted([FromBody] IdData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                Guid id = Guid.Parse(pData.Id);

                CodeCreatorModel ccm = FlowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, id) as CodeCreatorModel;

                if (ccm.Parent != null)
                {
                    ccm.Parent.CodeCreatorModels.ForEach(l => l.Value.RemoveAll(c => c == ccm));
                }
                else
                {
                    wfDefModel.CodeCreatorModel = null;
                }

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        [HttpPost]
        public JsonResult CodeCreatorCreated([FromBody] CreateCodeCreatorData pData)
        {
            try
            {
                DictionaryResponse ret = new DictionaryResponse();
                ret.IsSuccess = true;

                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                Guid? destContainer = null;
                Guid? destAfter = null;

                if (pData.DestinationContainerId != null)
                    destContainer = Guid.Parse(pData.DestinationContainerId);

                if (pData.DestinationAfterId != null)
                    destAfter = Guid.Parse(pData.DestinationAfterId);

                int? sequenceIndex = null;
                if (int.TryParse(pData.SequenceIndex, out int sindex))
                    sequenceIndex = sindex;



                var factory = Program.CoreflowInstance.CodeCreatorStorage.GetAllFactories().Single(f => f.Identifier == pData.CustomFactory);

                var tmpcc = factory.Create();


                CodeCreatorModel modes = new CodeCreatorModel()
                {
                    Type = pData.Type,
                    CustomFactory = pData.CustomFactory,
                    Identifier = Guid.NewGuid(),
                    DisplayName = tmpcc.GetDisplayName(),
                    Category = tmpcc.GetCategory(),
                    IconClass = tmpcc.GetIconClassName()
                };

                ret.Message = modes.Identifier.ToString();

                ICodeCreator cc = CodeCreatorModelHelper.CreateCode(modes, null);

                if (cc is IParametrized pm)
                {
                    modes.Parameters = pm.GetParameters().ConvertToModel();
                    modes.Arguments = new List<ArgumentModel>();

                    ret.ListValues = new List<ListEntry>();

                    foreach (var para in modes.Parameters)
                    {
                        Guid id = Guid.NewGuid();
                        ret.ListValues.Add(new ListEntry(id.ToString(), para.Name));
                        modes.Arguments.Add(new ArgumentModel(id, para.Name, ""));
                    }
                }

                if (cc is ICodeCreatorContainerCreator container)
                {
                    modes.CodeCreatorModels = new Dictionary<int, List<CodeCreatorModel>>();
                    modes.SequenceCount = container.SequenceCount;
                }


                UpdateCodeCreatorModel(wfDefModel, destAfter, destContainer, sequenceIndex, modes);

                FlowDefinitionModelStorage.StoreModel(wfDefModel, false);

                return Json(ret);
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        private static void UpdateCodeCreatorModel(FlowDefinitionModel pWfDefModel, Guid? pDestinationAfterId, Guid? pDestContainer, int? pDestSequenceIndex, CodeCreatorModel sourceModel)
        {
            if (pDestinationAfterId.HasValue) //insert cc after something
            {
                CodeCreatorModel destModel = FlowDefinitionModelIdentifiableHelper.FindIIdentifiable(pWfDefModel, pDestinationAfterId.Value) as CodeCreatorModel;

                List<CodeCreatorModel> codecreators = new List<CodeCreatorModel>();

                if (sourceModel.Parent != null)
                    sourceModel.Parent.CodeCreatorModels.ForEach(l => l.Value.RemoveAll(c => c == sourceModel));

                sourceModel.Parent = destModel.Parent;

                var sequenceEntry = destModel.Parent.CodeCreatorModels.First(m => m.Value.Any(x => x.Identifier == pDestinationAfterId));

                List<CodeCreatorModel> sequence = sequenceEntry.Value;

                foreach (var entry in sequence)
                {
                    codecreators.Add(entry);

                    if (entry.Identifier == pDestinationAfterId)
                    {
                        codecreators.Add(sourceModel);
                    }
                }

                destModel.Parent.CodeCreatorModels[sequenceEntry.Key] = codecreators;
            }
            else if (pDestContainer.HasValue) //insert cc on first entry of an container
            {
                CodeCreatorModel destContainerModel = FlowDefinitionModelIdentifiableHelper.FindIIdentifiable(pWfDefModel, pDestContainer.Value) as CodeCreatorModel;

                if (sourceModel.Parent != null)
                    sourceModel.Parent.CodeCreatorModels.ForEach(l => l.Value.RemoveAll(c => c == sourceModel));

                sourceModel.Parent = destContainerModel;

                if (destContainerModel.SequenceCount < pDestSequenceIndex.Value || pDestSequenceIndex.Value < 0)
                    throw new Exception("pDestSequenceIndex invalid");

                List<CodeCreatorModel> sequence = new List<CodeCreatorModel>();

                sequence.Add(sourceModel);

                if (destContainerModel.CodeCreatorModels.ContainsKey(pDestSequenceIndex.Value) && destContainerModel.CodeCreatorModels[pDestSequenceIndex.Value].Count > 0)
                {
                    foreach (var entry in destContainerModel.CodeCreatorModels[pDestSequenceIndex.Value])
                    {
                        sequence.Add(entry);
                    }
                }

                destContainerModel.CodeCreatorModels.Remove(pDestSequenceIndex.Value);
                destContainerModel.CodeCreatorModels.Add(pDestSequenceIndex.Value, sequence);
            }
            else //insert cc if wf is empty
            {
                if (pWfDefModel.CodeCreatorModel != null)
                    throw new InvalidOperationException("CodeCreatorModel must be null");

                pWfDefModel.CodeCreatorModel = sourceModel;
            }
        }

        [HttpPost]
        public JsonResult GetGeneratedCode([FromBody] IdData pData)
        {
            try
            {
                FlowDefinitionModel wfDefModel = FlowDefinitionModelStorage.GetModel(pData.FlowIdentifier);

                FlowDefinition wfDef = FlowDefinitionModelMappingHelper.GenerateFlowDefinition(wfDefModel);

                FlowCode code = wfDef.GenerateFlowCode();

                return Json(new Response(true, code.FormattedCode));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }
    }
}
