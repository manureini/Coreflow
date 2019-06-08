using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Coreflow.Helper
{
    public static class ReferenceHelper
    {
        private static string mDotnetRootPath;
        private static string mRefRootPath;

        private static Dictionary<string, MetadataReference> mReferenceCache = new Dictionary<string, MetadataReference>();

        private static object mLocker = new object();

        static ReferenceHelper()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                mDotnetRootPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "dotnet") + Path.DirectorySeparatorChar;
            }
            else
            {
                mDotnetRootPath = Environment.GetEnvironmentVariable("DOTNET_ROOT");
            }

            mRefRootPath = Path.Combine(mDotnetRootPath, "packs") + Path.DirectorySeparatorChar;
        }

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
            lock (mLocker)
            {
                var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
                var coreDir = Directory.GetParent(dd);

                var additional = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && a.Location != string.Empty)
                .Select(a =>
                {
                    try
                    {
                        string location = a.Location;

                        if (mReferenceCache.ContainsKey(location))
                            return mReferenceCache[location];

                        string referenceAssembly = FindReferenceAssemblyIfNeeded(location);

                        if (referenceAssembly == null)
                            return null;

                        var reference = MetadataReference.CreateFromFile(referenceAssembly);

                        mReferenceCache.Add(location, reference);

                        return reference;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    return null;
                }).Where(a => a != null);

                return (additional).Distinct().ToArray(); //make ToArray here because of lock
            }
        }


    }
}
