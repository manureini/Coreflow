using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Web.Extensions;
using Coreflow.Web.Models;
using Coreflow.Web.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Coreflow.Web.Controllers
{
    [Route("Home/Editor/Action/[action]")]
    [Authorize]
    public class EditorActionsController : Controller
    {

        [HttpPost]
        public JsonResult RunWorkflow()
        {
            string serialized = HttpContext.Session.GetString("WorkflowModel");
            WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

            WorkflowDefinition wfDef = WorkflowDefinitionModelMappingHelper.GenerateWorkflowDefinition(wfDefModel);

            //TODO
            Program.CoreflowInstance.WorkflowDefinitionStorage.Remove(wfDef.Identifier);
            Program.CoreflowInstance.WorkflowDefinitionStorage.Add(wfDef);

            WorkflowInvokeResult invokeResult = WorkflowInvoker.Invoke(wfDef);

            string result = invokeResult.ExecutedInstance.GetType().GetField("result").GetValue(invokeResult.ExecutedInstance) as string;

            return Json(new Response(false, result));
        }

        [HttpPost]
        public JsonResult ParameterTextChanged([FromBody] ParameterChangedData pData)
        {
            try
            {
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                CodeCreatorModel ccmodel = WorkflowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, Guid.Parse(pData.CreatorGuid)) as CodeCreatorModel;

                if (ccmodel.Arguments == null)
                    ccmodel.Arguments = new List<ArgumentModel>();

                Guid parameterGuid = Guid.Parse(pData.ParameterGuid);

                ArgumentModel am = ccmodel.Arguments.FirstOrDefault(a => a.Guid == parameterGuid);

                if (am == null)
                    throw new Exception("No Argument with Guid " + parameterGuid + " found!");

                am.Code = pData.NewValue;

                WorkflowDefinition wfDef = WorkflowDefinitionModelMappingHelper.GenerateWorkflowDefinition(wfDefModel);
                WorkflowCode code = wfDef.GenerateWorkflowCode();
                WorkflowCompileResult result = code.Compile();

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

                return Json(new GuidListResponse(true, "ok", result.ErrorCodeCreators));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        [HttpPost]
        public JsonResult UserDisplayNameChanged([FromBody] UserDisplayNameChangedData pData)
        {
            try
            {
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                CodeCreatorModel ccmodel = WorkflowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, Guid.Parse(pData.CreatorGuid)) as CodeCreatorModel;

                ccmodel.UserDisplayName = pData.NewValue;

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }


        [HttpPost]
        public JsonResult WorkflowReferencedAssemblyChanged([FromBody] WorkflowReferencedAssemblyChangedData pData)
        {
            try
            {
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                if (pData.AddValue)
                {
                    Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == pData.Value);

                    if (asm == null)
                    {
                        return Json(new Response(false, $"Assembly {pData.Value} not found!"));
                    }

                    wfDefModel.ReferencedAssemblies.Add(asm.FullName);
                }
                else
                {
                    wfDefModel.ReferencedAssemblies.Remove(pData.Value);
                }

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

                return Json(new Response(true, "ok"));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }


        [HttpPost]
        public JsonResult WorkflowReferencedNamespaceChanged([FromBody] WorkflowReferencedNamespaceChangedData pData)
        {
            try
            {
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                if (pData.AddValue)
                {
                    wfDefModel.ReferencedNamespaces.Add(pData.Value);
                }
                else
                {
                    wfDefModel.ReferencedNamespaces.Remove(pData.Value);
                }

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

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
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                Guid source = Guid.Parse(pData.SourceId);
                Guid destContainer = Guid.Parse(pData.DestinationContainerId);
                Guid? destAfter = null;

                int? sequenceIndex = null;
                if (int.TryParse(pData.SequenceIndex, out int sindex))
                    sequenceIndex = sindex;

                if (pData.DestinationAfterId != null)
                    destAfter = Guid.Parse(pData.DestinationAfterId);

                CodeCreatorModel sourceModel = WorkflowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, source) as CodeCreatorModel;

                UpdateCodeCreatorModel(wfDefModel, destAfter, destContainer, sequenceIndex, sourceModel);

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

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
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                Guid id = Guid.Parse(pData.Id);

                CodeCreatorModel ccm = WorkflowDefinitionModelIdentifiableHelper.FindIIdentifiable(wfDefModel, id) as CodeCreatorModel;

                if (ccm.Parent != null)
                {
                    ccm.Parent.CodeCreatorModels.ForEach(l => l.Value.RemoveAll(c => c == ccm));
                }
                else
                {
                    wfDefModel.CodeCreatorModel = null;
                }

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

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
                GuidListResponse ret = new GuidListResponse();
                ret.IsSuccess = true;

                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                Guid? destContainer = null;
                Guid? destAfter = null;

                if (pData.DestinationContainerId != null)
                    destContainer = Guid.Parse(pData.DestinationContainerId);

                if (pData.DestinationAfterId != null)
                    destAfter = Guid.Parse(pData.DestinationAfterId);

                int? sequenceIndex = null;
                if (int.TryParse(pData.SequenceIndex, out int sindex))
                    sequenceIndex = sindex;

                CodeCreatorModel modes = new CodeCreatorModel()
                {
                    Type = pData.Type,
                    DisplayName = pData.Type,
                    Identifier = Guid.NewGuid(),
                };

                ret.Message = modes.Identifier.ToString();

                ICodeCreator cc = CodeCreatorModelHelper.CreateCode(modes, null);

                if (cc is IParametrized)
                {
                    IParametrized pm = cc as IParametrized;

                    modes.Parameters = pm.GetParameters().ConvertToModel();
                    modes.Arguments = new List<ArgumentModel>();

                    ret.ListValues = new List<GuidEntry>();

                    foreach (var para in modes.Parameters)
                    {
                        Guid id = Guid.NewGuid();
                        ret.ListValues.Add(new GuidEntry(id, para.Name));
                        modes.Arguments.Add(new ArgumentModel(id, para.Name, ""));
                    }
                }

                if (cc is ICodeCreatorContainerCreator container)
                {
                    modes.CodeCreatorModels = new Dictionary<int, List<CodeCreatorModel>>();
                    modes.SequenceCount = container.SequenceCount;
                }

                UpdateCodeCreatorModel(wfDefModel, destAfter, destContainer, sequenceIndex, modes);

                serialized = WorkflowDefinitionModelSerializer.Serialize(wfDefModel);
                HttpContext.Session.SetString("WorkflowModel", serialized);

                return Json(ret);
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }

        private static void UpdateCodeCreatorModel(WorkflowDefinitionModel pWfDefModel, Guid? pDestinationAfterId, Guid? pDestContainer, int? pDestSequenceIndex, CodeCreatorModel sourceModel)
        {
            if (pDestinationAfterId.HasValue) //insert cc after something
            {
                CodeCreatorModel destModel = WorkflowDefinitionModelIdentifiableHelper.FindIIdentifiable(pWfDefModel, pDestinationAfterId.Value) as CodeCreatorModel;

                List<CodeCreatorModel> codecreators = new List<CodeCreatorModel>();

                if (sourceModel.Parent != null)
                    sourceModel.Parent.CodeCreatorModels.ForEach(l => l.Value.RemoveAll(c => c == sourceModel));

                sourceModel.Parent = destModel.Parent;

                var sequenceEntry = destModel.Parent.CodeCreatorModels.First(m => m.Value.Any(x => x.Identifier == pDestinationAfterId));

                List<CodeCreatorModel> sequence = sequenceEntry.Value;

                /*
                int afterIndex = sequence.FindIndex(c => c.Identifier == pDestinationAfterId);

                if (afterIndex > 0)
                    afterIndex--;

                sequence.Insert(afterIndex, sourceModel);
                */


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
                CodeCreatorModel destContainerModel = WorkflowDefinitionModelIdentifiableHelper.FindIIdentifiable(pWfDefModel, pDestContainer.Value) as CodeCreatorModel;

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
                string serialized = HttpContext.Session.GetString("WorkflowModel");
                WorkflowDefinitionModel wfDefModel = WorkflowDefinitionModelSerializer.DeSerialize(serialized);

                WorkflowDefinition wfDef = WorkflowDefinitionModelMappingHelper.GenerateWorkflowDefinition(wfDefModel);

                WorkflowCode code = wfDef.GenerateWorkflowCode();

                return Json(new Response(true, code.Code));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.ToString()));
            }
        }
    }
}
