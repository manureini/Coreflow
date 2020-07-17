using Coreflow.Objects;
using Coreflow.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Interfaces
{
    public class RuntimeFlowManager
    {
        protected Dictionary<Guid, FlowInstanceFactory> mFactories = new Dictionary<Guid, FlowInstanceFactory>();

        protected object mLocker = new object();

        protected CoreflowRuntime mCoreflow;

        public RuntimeFlowManager(CoreflowRuntime pCoreflow)
        {
            mCoreflow = pCoreflow;
        }


        public void UpdateFactories(Assembly pAssembly)
        {
            lock (mLocker)
            {
                IEnumerable<Type> flows = pAssembly.GetTypes().Where(t => typeof(ICompiledFlow).IsAssignableFrom(t));

                foreach (var flowtype in flows)
                {
                    var attribute = flowtype.GetCustomAttribute<FlowIdentifierAttribute>();
                    mFactories.Add(attribute.Identifier, new FlowInstanceFactory(mCoreflow, attribute.Identifier, flowtype));
                }
            }
        }

        public FlowInstanceFactory GetFactory(Guid pFlowIdentifier)
        {
            lock (mLocker)
            {
                return mFactories[pFlowIdentifier];
            }
        }
    }
}
