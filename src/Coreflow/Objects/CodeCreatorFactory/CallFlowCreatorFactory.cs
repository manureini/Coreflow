using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Objects.CodeCreatorFactory
{
    public class CallFlowCreatorFactory : ICodeCreatorFactory
    {
        public static string GetIdentifier(Guid pFlowIdentifier)
        {
            return typeof(CallFlowCreatorFactory).FullName + "_" + pFlowIdentifier;
        }

        public string Identifier => GetIdentifier(FlowIdentifier);

        public Guid FlowIdentifier { get; }

        public string FlowName { get; set; }

        public string FlowIcon { get; set; }

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
