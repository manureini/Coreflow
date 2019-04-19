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

        public CallFlowCreatorFactory(Guid pFlowIdentifier, string pFlowName, string pFlowIcon)
        {
            FlowIdentifier = pFlowIdentifier;
            FlowName = pFlowName;
            FlowIcon = pFlowIcon;
        }

        public ICodeCreator Create()
        {
            return new CallFlowCreator(FlowIdentifier, FlowName, FlowIcon)
            {
                FactoryIdentifier = Identifier
            };
        }
    }
}
