using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Runtime.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Coreflow.Runtime.Storage
{
    public class PreCompiledAssemblyFlowDefinitionStorage : IFlowDefinitionStorage
    {
        protected Dictionary<Guid, IFlowDefinition> mFlowDefinitions = new Dictionary<Guid, IFlowDefinition>();

        public Assembly FlowAssembly { get; protected set; }

        public PreCompiledAssemblyFlowDefinitionStorage(Assembly pAssembly)
        {
            FlowAssembly = pAssembly;

            IEnumerable<Type> flows = pAssembly.GetTypes().Where(t => typeof(ICompiledFlow).IsAssignableFrom(t));

            foreach (var flowtype in flows)
            {
                var attribute = flowtype.GetCustomAttribute<FlowIdentifierAttribute>();

                mFlowDefinitions.Add(attribute.Identifier, new SimpleFlowDefinition()
                {
                    Identifier = attribute.Identifier,
                    Name = attribute.Name
                });
            }
        }

        public IFlowDefinition Get(Guid pIdentifier)
        {
            if (mFlowDefinitions.ContainsKey(pIdentifier))
                return mFlowDefinitions[pIdentifier];

            return null;
        }

        public IEnumerable<IFlowDefinition> GetDefinitions()
        {
            return mFlowDefinitions.Values;
        }

        public void Remove(Guid pIdentifier)
        {
            throw new NotSupportedException();
        }

        public void Add(IFlowDefinition pFlowDefinition)
        {
            throw new NotSupportedException();
        }

        public void SetCoreflow(CoreflowRuntime pCoreflow)
        {
        }

        public void Dispose()
        {
        }
    }
}
