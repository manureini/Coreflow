using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow
{
    internal class FlowBuilderHelper
    {
        private const string INSTANCE_ID_PARAMETER_NAME = "InstanceId";

        public static IEnumerable<FlowCode> GenerateFlowCode(IEnumerable<FlowDefinition> pFlows)
        {
            return pFlows.Select(w => GenerateFlowCode(w));
        }

        public static FlowCode GenerateFlowCode(FlowDefinition pFlow)
        {
            FlowCode ret = new FlowCode();

            var wfReferencesDict = ReferenceHelper.GetMetadataReferences(pFlow);

            ret.ReferencedAssemblies = wfReferencesDict.Values;


            FlowCodeWriter cw = new FlowCodeWriter();

            FlowBuilderContext context = new FlowBuilderContext(cw, ret.ReferencedAssemblies.ToList());


            cw.AppendLineTop("namespace DynamicGeneratedFlow" + pFlow.Identifier.ToString().ToVariableName() + " {");

            foreach (string import in pFlow.ReferencedNamespaces.Distinct())
            {
                cw.AppendLineTop($"using {import};");
            }

            cw.AppendLineTop();

            cw.WriteIdentifierTagTop(pFlow);
            cw.WriteContainerTagTop(pFlow);

            //Currently idk which letters needs an escape
            cw.AppendLineTop("public class wf_" + pFlow.Name.Replace(" ", "") + " : " + nameof(ICompiledFlow) + "  {");

            cw.AppendLineTop();

            cw.AppendLineTop($"public Guid {INSTANCE_ID_PARAMETER_NAME} = Guid.NewGuid();");

            cw.AppendLineTop();

            if (pFlow.Arguments != null)
                foreach (FlowArguments parameter in pFlow.Arguments)
                {
                    if (!wfReferencesDict.ContainsKey(parameter.Type.Assembly))
                        throw new ArgumentException($"Flow has parameter with type {parameter.Type}, but does not reference assembly {parameter.Type.Assembly.FullName}");

                    string value = $"default({parameter.Type.FullName})";

                    if (parameter.Expression != null && !string.IsNullOrWhiteSpace(parameter.Expression))
                        value = parameter.Expression;

                    cw.AppendLineTop($"public {parameter.Type.FullName} {parameter.Name} = {value};");
                }

            cw.AppendLineTop();

            cw.AppendLineTop("public void Run() { ");


            cw.AppendLineBottom("}"); //Close Run

            cw.AppendLineBottom("}"); //Close Class

            cw.AppendLineBottom("}"); //Close Namespace


            if (pFlow.CodeCreator != null)
            {
                if (pFlow.CodeCreator is IVariableCreator ccv)
                    ccv.Initialize(context, cw);

                pFlow.CodeCreator.ToCode(context, cw);
            }

            ret.Code = cw.ToString();
            return ret;
        }

        public static IVariableCreator GetVariableCreatorInScope(ICodeCreatorContainerCreator pContainer, ICodeCreator pCodeCreator, Func<IVariableCreator, bool> pFilter)
        {
            if (pContainer == null)
                return null;

            List<ICodeCreator> ccontainer = pContainer.CodeCreators.FirstOrDefault(cl => cl.Contains(pCodeCreator));

            IVariableCreator found = ccontainer.Select(v => v as IVariableCreator).Where(v => v != null).FirstOrDefault(pFilter);
            if (found != null)
                return found;

            return GetVariableCreatorInScope(pContainer.ParentContainerCreator, pContainer, pFilter);
        }



        /*
        public static IVariableCreator GetVariableCreatorInScope(ICodeCreatorContainerCreator pContainer, ICodeCreator pCodeCreator, Func<IVariableCreator, bool> pFilter)
        {
            if (pContainer == null)
                return null;

            List<ICodeCreator> ccontainer = pContainer.CodeCreators.FirstOrDefault(
                cl => cl.Contains(pCodeCreator) ||
                cl.Select(v => v as IParametrized).Where(v => v != null).Any(c => c.Arguments.Contains(pCodeCreator)));

            IVariableCreator found = ccontainer.Select(v => v as IVariableCreator).Where(v => v != null).FirstOrDefault(pFilter);
            if (found != null)
                return found;

            var parametrizedCc = ccontainer.Select(v => v as IParametrized).Where(v => v != null);

            foreach (var cc in parametrizedCc)
            {
                found = cc.Arguments.Select(a => a as IVariableCreator).Where(v => v != null).FirstOrDefault(pFilter);

                if (found == pCodeCreator)
                    return null;

                if (found != null)
                    return found;
            }

            return GetVariableCreatorInScope(pContainer.ParentContainerCreator, pContainer, pFilter);
        } */


    }
}
