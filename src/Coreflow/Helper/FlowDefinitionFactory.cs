using Coreflow.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Helper
{
    public class FlowDefinitionFactory
    {
        private Coreflow mCoreflow;


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

        internal FlowDefinitionFactory(Coreflow pCoreflow)
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
            pNameSpaces.ForEach(a => AddDefaultReferencedNamespace(a));
        }

        public FlowDefinition Create(string pName)
        {
            FlowDefinition w = new FlowDefinition(mCoreflow)
            {
                Name = pName,
                ReferencedNamespaces = DefaultReferencedNamespaces,
                Arguments = new List<FlowArguments>(),
            };
            return w;
        }
    }
}
