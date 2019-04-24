using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Objects.CodeCreatorFactory
{
    public class CallFlowCreatorFactory : ICodeCreatorFactory
    {
        public string Identifier => typeof(CallFlowCreatorFactory).FullName + "_" + FlowIdentifier;

        public Guid FlowIdentifier { get; }

        public string FlowName { get; }

        public string FlowIcon { get; }

        public List<FlowArguments> Arguments { get; }

        public CallFlowCreatorFactory(FlowDefinition pFlowDefinition)
        {
            FlowIdentifier = pFlowDefinition.Identifier;
            FlowName = pFlowDefinition.Name;
            FlowIcon = pFlowDefinition.Icon;
            Arguments = pFlowDefinition.Arguments;
        }

        public ICodeCreator Create()
        {
            return new CallFlowCreator(FlowIdentifier, FlowName, FlowIcon, Arguments)
            {
                FactoryIdentifier = Identifier
            };
        }
    }
}
