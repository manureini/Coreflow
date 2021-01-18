using Coreflow.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Helper
{
    public class FlowDefinitionFactory
    {
        private CoreflowService mCoreflow;


        public List<string> DefaultReferencedNamespaces = new List<string>()
        {
            "System",
            "System.Collections.Generic",
        //    "System.Collections.Specialized",
            "System.IO",
            "System.Linq",
            "System.Reflection",
            "System.Runtime",
   //         nameof(Coreflow)
        };

        internal FlowDefinitionFactory(CoreflowService pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void AddDefaultReferencedNamespace(string pNameSpace)
        {
            if (!DefaultReferencedNamespaces.Contains(pNameSpace))
                DefaultReferencedNamespaces.Add(pNameSpace);
        }

        public void AddDefaultReferencedNamespace(IEnumerable<string> pNameSpaces)
        {
            foreach (var ns in pNameSpaces)
            {
                AddDefaultReferencedNamespace(ns);
            }
        }

        public FlowDefinition Create(string pName)
        {
            var flowDefinition = new FlowDefinition(mCoreflow)
            {
                Name = pName,
                ReferencedNamespaces = DefaultReferencedNamespaces,
                Arguments = new List<FlowArgument>(),
            };
            mCoreflow.FlowDefinitionStorage.Add(flowDefinition);
            return flowDefinition;
        }
    }
}
