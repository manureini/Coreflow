using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Coreflow.Helper
{
    public static class ReferenceHelper
    {
        private static string mDotnetRootPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "dotnet") + Path.DirectorySeparatorChar;
        private static string mRefRootPath = Path.Combine(mDotnetRootPath, "packs") + Path.DirectorySeparatorChar;

        private static string FindReferenceAssemblyIfNeeded(string pRuntimeAssembly)
        {
            if (!pRuntimeAssembly.StartsWith(mDotnetRootPath))
                return pRuntimeAssembly;

            if (pRuntimeAssembly.Contains(".Private."))
                return null;

            string dllFileName = Path.GetFileName(pRuntimeAssembly);

            var refFiles = Directory.GetFiles(mRefRootPath, dllFileName, SearchOption.AllDirectories).Where(f =>
            {
                var runtimeasm = AssemblyName.GetAssemblyName(pRuntimeAssembly);
                var refAsm = AssemblyName.GetAssemblyName(f);

                if (runtimeasm.Version != refAsm.Version)
                    return false;

                if (Encoding.UTF8.GetString(runtimeasm.GetPublicKey()) != Encoding.UTF8.GetString(refAsm.GetPublicKey()))
                    return false;

                return true;
            });

            if (refFiles.Count() > 1)
            {
                Console.WriteLine($"WARNING: Search for referenced assembly {dllFileName} in {mRefRootPath} has mutiple results");
            }

            string refPath = refFiles.FirstOrDefault();

            if (refPath != null && File.Exists(refPath))
            {
                return refPath;
            }

            return pRuntimeAssembly;
        }

        public static IEnumerable<MetadataReference> GetMetadataReferences()
        {
            var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
            var coreDir = Directory.GetParent(dd);

            var additional = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && a.Location != string.Empty)
            .Select(a =>
            {
                try
                {
                    string referenceAssembly = FindReferenceAssemblyIfNeeded(a.Location);

                    if (referenceAssembly == null)
                        return null;

                    return MetadataReference.CreateFromFile(referenceAssembly);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return null;
            }).Where(a => a != null);


            var result = (additional).Distinct();

            return result;
        }


    }
}
