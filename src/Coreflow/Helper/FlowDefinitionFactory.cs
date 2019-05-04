using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

        /*
        private Assembly FindAssembly(string pFilename)
        {
            Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => !a.IsDynamic && Path.GetFileName(a.Location) == pFilename);
            if (asm != null)
                return asm;

            asm = Assembly.Load(pFilename.Replace(".dll", ""));

            if (asm != null)
                return asm;

            throw new Exception($"Assembly with filename {pFilename} not found!");
        }*/


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

            mCoreflow.FlowDefinitionStorage.Add(w);
            return w;
        }
    }
}
