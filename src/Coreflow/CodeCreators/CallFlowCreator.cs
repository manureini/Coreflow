using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Runtime;
using System;
using System.Collections.Generic;

namespace Coreflow.CodeCreators
{
    public class CallFlowCreator : IVariableCreator, ICustomFactoryCodeCreator, IParametrized, IUiDesignable
    {
        public string Name => FlowName;

        public string Icon { get; set; }

        public string Category => "Flows";

        public string VariableIdentifier => Guid.NewGuid().ToString();

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public string FlowName { get; set; }

        public string FlowVariableName { get; set; }

        public List<IArgument> Arguments { get; set; } = new List<IArgument>();


        public CodeCreatorParameter[] Parameters { get; set; }


        public CallFlowCreator()
        {
        }

        public CallFlowCreator(Guid pFlowId, string pFlowName, string pFlowIcon, List<FlowArgument> pArguments)
        {
            FlowVariableName = pFlowId.ToString().ToVariableName();
            FlowName = pFlowName;
            Icon = pFlowIcon;

            List<CodeCreatorParameter> parameterList = new List<CodeCreatorParameter>();

            var args = pArguments ?? new List<FlowArgument>();

            foreach (var entry in args)
            {
                parameterList.Add(new CodeCreatorParameter()
                {
                    Direction = entry.Direction,
                    Name = entry.Name,
                    Type = entry.Type
                });
            }

            Parameters = parameterList.ToArray();
        }

        public void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pBuilderContext.CreateLocalVariableName(this);
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null)
        {
            pBuilderContext.UpdateCurrentSymbols();
            pCodeWriter.WriteIdentifierTagTop(this);

            string flowInstanceVariableName = pBuilderContext.GetLocalVariableName(this);

            pCodeWriter.AppendLineTop($"var {flowInstanceVariableName} = new {FlowBuilderHelper.FLOW_NAMESPACE_PREFIX}{FlowVariableName}.{FlowBuilderHelper.FLOW_CLASS_PREFIX}{FlowVariableName}();");

            string dictVariableName = flowInstanceVariableName + "_1";

            pCodeWriter.AppendLineTop($"var {dictVariableName} = new Dictionary<string, object>();");

            foreach (var entry in Arguments)
            {
                if (string.IsNullOrWhiteSpace(entry.Code))
                    continue;

                pCodeWriter.AppendLineTop($"{dictVariableName}.Add(\"{entry.Name}\",");
                entry.ToCode(pBuilderContext, pCodeWriter, pParentContainer);
                pCodeWriter.AppendLineTop($");");
            }

            pCodeWriter.AppendLineTop($"{flowInstanceVariableName}.SetArguments({dictVariableName});");

            pCodeWriter.AppendLineTop($"{flowInstanceVariableName}.{nameof(ICompiledFlow.CoreflowInstace)} = this.{nameof(ICompiledFlow.CoreflowInstace)};");
            pCodeWriter.AppendLineTop($"{flowInstanceVariableName}.{nameof(ICompiledFlow.ArgumentInjectionStore)} = this.{nameof(ICompiledFlow.ArgumentInjectionStore)};");
            pCodeWriter.AppendLineTop($"{flowInstanceVariableName}.{nameof(ICompiledFlow.Logger)} = this.{nameof(ICompiledFlow.Logger)};");

            pCodeWriter.AppendLineTop($"{flowInstanceVariableName}.Run();");
        }

        public CodeCreatorParameter[] GetParameters()
        {
            return Parameters ?? new CodeCreatorParameter[0];
        }
    }
}
