using Coreflow.Interfaces;
using Coreflow.Objects;
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

        public static CodeCreatorModel CreateModel(ICodeCreator pCodeCreator, CodeCreatorModel pParent, WorkflowDefinition pWorkflowDefinition)
        {
            if (pCodeCreator == null)
                return null;

            CodeCreatorModel ret = new CodeCreatorModel
            {
                Identifier = pCodeCreator.Identifier,
                DisplayName = pCodeCreator.GetDisplayName(),
                IconClass = pCodeCreator.GetIconClassName(),
                Type = pCodeCreator.GetType().AssemblyQualifiedName,
                Parent = pParent
            };

            if (pWorkflowDefinition != null && pWorkflowDefinition.Metadata != null && pWorkflowDefinition.Metadata.ContainsKey(ret.Identifier))
            {
                var metadata = pWorkflowDefinition.Metadata[ret.Identifier];

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

            if (pCodeCreator is ICodeCreatorContainerCreator)
            {
                ret.CodeCreatorModelsFirst = new List<CodeCreatorModel>();
                ICodeCreatorContainerCreator container = pCodeCreator as ICodeCreatorContainerCreator;

                foreach (var listcc in container.CodeCreators)
                {
                    foreach (ICodeCreator cc in listcc)
                        ret.CodeCreatorModelsFirst.Add(CreateModel(cc, ret, pWorkflowDefinition));
                }
            }

            return ret;
        }

        public static ICodeCreator CreateCode(CodeCreatorModel pCodeCreatorModel, WorkflowDefinition pWorkflowDefinition)
        {
            if (pCodeCreatorModel == null)
                return null;

            Type type = Type.GetType(pCodeCreatorModel.Type);
            ICodeCreator ret = Activator.CreateInstance(type) as ICodeCreator;

            ret.Identifier = pCodeCreatorModel.Identifier;

            if (pWorkflowDefinition != null && pCodeCreatorModel.UserDisplayName != null)
            {
                //TODO helper
                //TODO init here?
                if (pWorkflowDefinition.Metadata == null)
                    pWorkflowDefinition.Metadata = new Dictionary<Guid, Dictionary<string, object>>();

                if (!pWorkflowDefinition.Metadata.ContainsKey(pCodeCreatorModel.Identifier))
                    pWorkflowDefinition.Metadata.Add(pCodeCreatorModel.Identifier, new Dictionary<string, object>());

                pWorkflowDefinition.Metadata[ret.Identifier].Remove(USER_DISPLAY_NAME);
                pWorkflowDefinition.Metadata[ret.Identifier].Add(USER_DISPLAY_NAME, pCodeCreatorModel.UserDisplayName);
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
                            parametrized.Arguments.Add(new InputExpressionCreator(argument.Name, argument.Code, argument.Guid));
                        else if (param.Direction == VariableDirection.Out)
                        {
                            bool isSimpleVariableName = !argument.Code.Trim().Contains(" ") && !argument.Code.Contains("\"");

                            if (isSimpleVariableName)
                                parametrized.Arguments.Add(new OutputVariableNameCreator(argument.Name, argument.Code, argument.Guid));

                            else parametrized.Arguments.Add(new OutputVariableCodeInlineCreator(argument.Name, argument.Code, argument.Guid));
                        }

                        //TODO InOut
                    }
                }
            }

            if (ret is ICodeCreatorContainerCreator container)
            {
                container.CodeCreators = new List<List<ICodeCreator>>();

                if (pCodeCreatorModel.CodeCreatorModelsFirst != null)
                {
                    List<ICodeCreator> first = new List<ICodeCreator>();

                    foreach (CodeCreatorModel ccmodel in pCodeCreatorModel.CodeCreatorModelsFirst)
                    {
                        first.Add(CreateCode(ccmodel, pWorkflowDefinition));
                    }

                    container.CodeCreators.Add(first);
                }
            }

            return ret;
        }

    }
}
