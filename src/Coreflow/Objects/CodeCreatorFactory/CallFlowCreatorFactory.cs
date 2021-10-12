﻿using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;

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

        public string FlowName => FlowDefinition.Name;

        public string FlowIcon => FlowDefinition.Icon;

        public List<FlowArgument> Arguments => FlowDefinition.Arguments;

        private WeakReference<FlowDefinition> mDefinition;
        private CoreflowService mCoreflow;

        public FlowDefinition FlowDefinition
        {
            get
            {
                if (mDefinition.TryGetTarget(out FlowDefinition fref))
                    return fref;

                return (FlowDefinition)mCoreflow.FlowDefinitionStorage.Get(FlowIdentifier);
            }
        }

        public Type[] OverrideableCustomTypes => null;

        public CallFlowCreatorFactory(CoreflowService pInstance, FlowDefinition pFlowDefinition)
        {
            mCoreflow = pInstance;
            mDefinition = new WeakReference<FlowDefinition>(pFlowDefinition);
            FlowIdentifier = pFlowDefinition.Identifier;
        }

        public ICodeCreator Create(Type[] pCustomTypes = null)
        {
            return new CallFlowCreator(FlowIdentifier, FlowName, FlowIcon, Arguments)
            {
                FactoryIdentifier = Identifier
            };
        }
    }
}
