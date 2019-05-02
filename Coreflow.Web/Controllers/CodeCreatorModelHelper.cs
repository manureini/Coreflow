using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Storage;
using Coreflow.Web.Extensions;
using Coreflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Web.Controllers
{
    public class CodeCreatorModelHelper
    {
        private const string USER_DISPLAY_NAME = "UserDisplayName";

        public static CodeCreatorModel CreateModel(ICodeCreator pCodeCreator, CodeCreatorModel pParent, FlowDefinition pFlowDefinition)
        {
            if (pCodeCreator == null)
                return null;

            CodeCreatorModel ret = new CodeCreatorModel
            {
                Identifier = pCodeCreator.Identifier,
                DisplayName = pCodeCreator.GetDisplayName(),
                IconClass = pCodeCreator.GetIconClassName(),
                Type = pCodeCreator.GetType().AssemblyQualifiedName,
                CustomFactory = (pCodeCreator is ICustomFactoryCodeCreator cfcc) ? cfcc.FactoryIdentifier : null,
                Parent = pParent
            };

            if (pFlowDefinition != null && pFlowDefinition.Metadata != null && pFlowDefinition.Metadata.ContainsKey(ret.Identifier))
            {
                var metadata = pFlowDefinition.Metadata[ret.Identifier];

                if (metadata.ContainsKey(USER_DISPLAY_NAME))
                    ret.UserDisplayName = metadata[USER_DISPLAY_NAME].ToString();
            }

            if (pCodeCreator is IParametrized parametrized)
            {
                //TODO validate Argument Names against Parameter names
                //The editor should be able to show a warning and ask the user to fix the consistency of arguments and parameters

                ret.Parameters = parametrized.GetParameters().ConvertToModel();

                if (parametrized.Arguments.Count == 0)
                {
                    ret.Arguments = new List<ArgumentModel>();
                    foreach (var entry in ret.Parameters)
                    {
                        ret.Arguments.Add(new ArgumentModel(Guid.NewGuid(), entry.Name, ""));
                    }
                }
                else
                {
                    //TODO more intelligent merge?
                    //merge old argument to new argument

                    ret.Arguments = parametrized.Arguments.Select(p => new ArgumentModel(p.Identifier, p.Name, p.Code)).ToList();
                }
            }

            if (pCodeCreator is ICodeCreatorContainerCreator container)
            {
                ret.CodeCreatorModels = new Dictionary<int, List<CodeCreatorModel>>();

                int i = 0;
                foreach (var listcc in container.CodeCreators)
                {
                    List<CodeCreatorModel> models = new List<CodeCreatorModel>();

                    foreach (ICodeCreator cc in listcc)
                        models.Add(CreateModel(cc, ret, pFlowDefinition));

                    ret.CodeCreatorModels.Add(i, models);
                    i++;
                }

                ret.SequenceCount = container.SequenceCount;
            }

            return ret;
        }

        public static ICodeCreator CreateCode(CodeCreatorModel pCodeCreatorModel, FlowDefinition pFlowDefinition)
        {
            if (pCodeCreatorModel == null)
                return null;

            ICodeCreatorFactory factory = Program.CoreflowInstance.CodeCreatorStorage.GetFactory(pCodeCreatorModel.Type, pCodeCreatorModel.CustomFactory);
            ICodeCreator ret = factory.Create();

            ret.Identifier = pCodeCreatorModel.Identifier;

            if (pFlowDefinition != null && pCodeCreatorModel.UserDisplayName != null)
            {
                //TODO helper
                //TODO init here?
                if (pFlowDefinition.Metadata == null)
                    pFlowDefinition.Metadata = new Dictionary<Guid, Dictionary<string, object>>();

                if (!pFlowDefinition.Metadata.ContainsKey(pCodeCreatorModel.Identifier))
                    pFlowDefinition.Metadata.Add(pCodeCreatorModel.Identifier, new Dictionary<string, object>());

                pFlowDefinition.Metadata[ret.Identifier].Remove(USER_DISPLAY_NAME);
                pFlowDefinition.Metadata[ret.Identifier].Add(USER_DISPLAY_NAME, pCodeCreatorModel.UserDisplayName);
            }

            if (ret is IParametrized parametrized)
            {
                if (pCodeCreatorModel.Parameters != null)
                {
                    if (pCodeCreatorModel.Parameters.Count != pCodeCreatorModel.Arguments.Count)
                        throw new Exception("Inconsistency between Parameters and Arguments");

                    for (int i = 0; i < pCodeCreatorModel.Parameters.Count; i++)
                    {
                        var param = pCodeCreatorModel.Parameters[i];
                        var argument = pCodeCreatorModel.Arguments[i];

                        if (param.Direction == VariableDirection.In)
                            parametrized.Arguments.Add(new InputExpressionCreator(argument.Name, argument.Code, argument.Guid, param.Type));
                        else if (param.Direction == VariableDirection.Out)
                        {
                            bool isSimpleVariableName = !argument.Code.Trim().Contains(" ") && !argument.Code.Contains("\"");

                            if (isSimpleVariableName)
                            {
                                argument.Name = argument.Name.Trim();

                                if (param.Type == typeof(LeftSideCSharpCode))
                                    parametrized.Arguments.Add(new LeftSideVariableNameCreator(argument.Name, argument.Code, argument.Guid));
                                else
                                    parametrized.Arguments.Add(new OutputVariableNameCreator(argument.Name, argument.Code, argument.Guid));
                            }
                            else
                                parametrized.Arguments.Add(new OutputVariableCodeInlineCreator(argument.Name, argument.Code, argument.Guid));
                        }
                        else //InOut
                        {
                            parametrized.Arguments.Add(new InputOutputVariableNameCreator(argument.Name, argument.Code, argument.Guid));
                        }
                    }
                }
            }

            if (ret is ICodeCreatorContainerCreator container)
            {
                container.CodeCreators = new List<List<ICodeCreator>>();

                if (pCodeCreatorModel.CodeCreatorModels != null)
                {
                    foreach (var ccModels in pCodeCreatorModel.CodeCreatorModels)
                    {
                        List<ICodeCreator> list = new List<ICodeCreator>();

                        foreach (CodeCreatorModel ccmodel in ccModels.Value)
                            list.Add(CreateCode(ccmodel, pFlowDefinition));

                        container.CodeCreators.Add(list);
                    }
                }
            }

            return ret;
        }
    }
}
