using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Helper
{
    public class ReferenceHelper
    {
        private const string DLL_FILE_EXTENSION = ".dll";

        private static Dictionary<string, MetadataReference> mReferences = new Dictionary<string, MetadataReference>();

        public static void ReloadReferences()
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
            {
                mReferences.Add(a.GetName().Name, MetadataReference.CreateFromFile(a.Location));
            }
        }

        public static IDictionary<Assembly, MetadataReference> GetMetadataReferences(WorkflowDefinition pWorkflowDefinition)
        {
            Dictionary<Assembly, MetadataReference> ret = new Dictionary<Assembly, MetadataReference>();

            foreach (string ns in pWorkflowDefinition.ReferencedNamespaces)
            {
                IEnumerable<Assembly> assemblies = Enumerable.Empty<Assembly>();

                if (pWorkflowDefinition.ReferencedAssemblies != null)
                    assemblies = pWorkflowDefinition.ReferencedAssemblies.Where(a => a.GetTypes().FirstOrDefault(t => t.Namespace == ns) != null);

                if (assemblies.Count() == 0)
                    assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetTypes().FirstOrDefault(t => t.Namespace == ns) != null);

                if (assemblies.Count() == 0)
                    throw new Exception("Could not find any Assembly which contains the following namespace: " + ns);

                foreach (Assembly assembly in assemblies)
                    if (!ret.ContainsKey(assembly))
                        ret.Add(assembly, MetadataReference.CreateFromFile(assembly.Location));
            }

            return ret;
        }


    }
}
