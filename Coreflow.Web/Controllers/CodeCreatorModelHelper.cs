using Coreflow.Helper;
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
        private const string USER_NOTE = "UserNote";
        private const string USER_COLOR = "UserColor";

        public static CodeCreatorModel CreateModel(ICodeCreator pCodeCreator, CodeCreatorModel pParent, FlowDefinition pFlowDefinition)
        {
            if (pCodeCreator == null)
                return null;

            CodeCreatorModel ret = new CodeCreatorModel
            {
                Identifier = pCodeCreator.Identifier,
                DisplayName = pCodeCreator.GetDisplayName(),
                IconClass = pCodeCreator.GetIconClassName(),
                Category = pCodeCreator.GetCategory(),
                Type = pCodeCreator.GetType().AssemblyQualifiedName,
                CustomFactory = (pCodeCreator is ICustomFactoryCodeCreator cfcc) ? cfcc.FactoryIdentifier : null,
                Parent = pParent
            };

            if (pFlowDefinition != null)
            {
                ret.UserDisplayName = (string)pFlowDefinition.GetMetadata(ret.Identifier, USER_DISPLAY_NAME);
                ret.UserNote = (string)pFlowDefinition.GetMetadata(ret.Identifier, USER_NOTE);
                ret.UserColor = (string)pFlowDefinition.GetMetadata(ret.Identifier, USER_COLOR);
            }

            if (pCodeCreator is IParametrized parametrized)
            {
                ret.Parameters = parametrized.GetParameters().ConvertToModel();

                if (parametrized.Arguments == null)
                    parametrized.Arguments = new List<IArgument>();

                ret.Arguments = parametrized.Arguments.Select(p => new ArgumentModel(p.Identifier, p.Name, p.Code)).ToList();
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

            if (pFlowDefinition != null)
            {
                pFlowDefinition.SetMetadata(ret.Identifier, USER_DISPLAY_NAME, pCodeCreatorModel.UserDisplayName);
                pFlowDefinition.SetMetadata(ret.Identifier, USER_NOTE, pCodeCreatorModel.UserNote);
                pFlowDefinition.SetMetadata(ret.Identifier, USER_COLOR, pCodeCreatorModel.UserColor);
            }

            if (ret is IParametrized parametrized)
            {
                if (parametrized.Arguments == null)
                    parametrized.Arguments = new List<IArgument>();

                foreach (var argument in pCodeCreatorModel.Arguments)
                {
                    var param = pCodeCreatorModel.Parameters.FirstOrDefault(p => p.Name == argument.Name);
                    parametrized.Arguments.Add(ArgumentHelper.CreateArgument(param, argument.Name, argument.Code, argument.Guid));
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
