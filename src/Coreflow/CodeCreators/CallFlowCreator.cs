using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.CodeCreators
{
    public class CallFlowCreator : IVariableCreator, ICustomFactoryCodeCreator, IUiDesignable
    {
        public string Name => FlowName;

        public string Icon { get; set; }

        public string VariableIdentifier => Guid.NewGuid().ToString();

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string FactoryIdentifier { get; set; }

        public string FlowName { get; set; }

        public string FlowVariableName { get; set; }

        public CallFlowCreator()
        {
        }

        public CallFlowCreator(Guid pFlowId, string pFlowName, string pFlowIcon)
        {
            FlowVariableName = pFlowId.ToString().ToVariableName();
            FlowName = pFlowName;
            Icon = pFlowIcon;
        }

        public void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pBuilderContext.CreateLocalVariableName(this);
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pParentContainer = null)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeWriter.AppendLineTop($"var {pBuilderContext.GetLocalVariableName(this)} = new {FlowBuilderHelper.FLOW_NAMESPACE_PREFIX}{FlowVariableName}.{FlowBuilderHelper.FLOW_CLASS_PREFIX}{FlowVariableName}();");

            //TODO parameter

            pCodeWriter.AppendLineTop($"{pBuilderContext.GetLocalVariableName(this)}.Run();");
        }
    }
}
