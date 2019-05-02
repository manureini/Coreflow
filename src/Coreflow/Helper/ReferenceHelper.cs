using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coreflow.Helper
{
    public class ReferenceHelper
    {
        /*
        private const string DLL_FILE_EXTENSION = ".dll";

        private static Dictionary<string, MetadataReference> mReferences = new Dictionary<string, MetadataReference>();

        public static void ReloadReferences()
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
            {
                mReferences.Add(a.GetName().Name, MetadataReference.CreateFromFile(a.Location));
            }
        }

        public static IDictionary<Assembly, MetadataReference> GetMetadataReferences(FlowDefinition pFlowDefinition)
        {
            Dictionary<Assembly, MetadataReference> ret = new Dictionary<Assembly, MetadataReference>();

            /*
            foreach (string ns in pFlowDefinition.ReferencedNamespaces)
            {
                IEnumerable<Assembly> assemblies = Enumerable.Empty<Assembly>();

                if (pFlowDefinition.ReferencedAssemblies != null)
                    assemblies = pFlowDefinition.ReferencedAssemblies.Where(a => a.GetTypes().FirstOrDefault(t => t.Namespace == ns) != null);

                if (assemblies.Count() == 0)
                    assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetTypes().FirstOrDefault(t => t.Namespace == ns) != null);

                if (assemblies.Count() == 0)
                    throw new Exception("Could not find any Assembly which contains the following namespace: " + ns);

                foreach (Assembly assembly in assemblies)
                    if (!ret.ContainsKey(assembly))
                        ret.Add(assembly, MetadataReference.CreateFromFile(assembly.Location));
            }*/

        /*
        foreach (var assembly in pFlowDefinition.ReferencedAssemblies)
        {
            ret.Add(assembly, MetadataReference.CreateFromFile(assembly.Location));
        }


        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!assembly.IsDynamic && assembly.Location != string.Empty)
                ret.Add(assembly, MetadataReference.CreateFromFile(assembly.Location));
        }

        return ret;
    }
    */

        public static IEnumerable<MetadataReference> GetMetadataReferences()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && a.Location != string.Empty)
                .Select(a => MetadataReference.CreateFromFile(a.Location));
        }


    }
}
