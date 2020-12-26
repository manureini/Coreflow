using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Coreflow.Helper
{
    public static class ReferenceHelper
    {
        private static string mDotnetRootPath;
        private static string mRefRootPath;

        private static Dictionary<string, MetadataReference> mReferenceCache = new Dictionary<string, MetadataReference>();

        private static object mLocker = new object();
        private static SemaphoreSlim mLoadLocker = new SemaphoreSlim(1, 1);

        static ReferenceHelper()
        {
            if (RuntimeInformation.OSArchitecture == Architecture.Wasm)
                return;

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

        public static async Task LoadReferencesFromWebAsync(HttpClient pHttpClient)
        {
            if (RuntimeInformation.OSArchitecture == Architecture.Wasm)
            {
                try
                {
                    await mLoadLocker.WaitAsync();

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic);

                    foreach (var assembly in assemblies)
                    {
                        if (assembly.ManifestModule.ScopeName.StartsWith(FlowCompilerHelper.FLOW_ASSEMBLY_PREFIX) ||
                            string.IsNullOrWhiteSpace(assembly.FullName) ||
                            assembly.FullName.StartsWith(FlowCompilerHelper.FLOW_ASSEMBLY_PREFIX))
                            continue;

                        if (mReferenceCache.ContainsKey(assembly.FullName))
                            continue;

                        var assemblyName = assembly.GetName().Name;

                        if (string.IsNullOrWhiteSpace(assemblyName) || assemblyName.StartsWith(FlowCompilerHelper.FLOW_ASSEMBLY_PREFIX))
                            continue;

                        var fileName = assemblyName + ".dll";

                        try
                        {
                            var stream = await pHttpClient.GetStreamAsync("/_framework/" + WebUtility.UrlEncode(fileName));
                            mReferenceCache.Add(assembly.FullName, MetadataReference.CreateFromStream(stream));
                        }
                        catch (Exception e)
                        {
                            mReferenceCache.Add(assembly.FullName, null);
                            Console.WriteLine(e);
                        }
                    }
                }
                finally
                {
                    mLoadLocker.Release();
                }
            }
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

            if (pRuntimeAssembly.Contains("netstandard.dll") && refFiles.Count() > 1)
            {
                refFiles = refFiles.Where(r => r.Contains("netcore"));
                Console.WriteLine($"WARNING: netstandard.dll has multiple resulsts force using reference with netcore");
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
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic);

                if (RuntimeInformation.OSArchitecture == Architecture.Wasm)
                {
                    var ret = assemblies.Select(a =>
                     {
                         if (a.ManifestModule.ScopeName.StartsWith(FlowCompilerHelper.FLOW_ASSEMBLY_PREFIX))
                             return null;

                         var location = a.FullName;
                         if (mReferenceCache.ContainsKey(location))
                             return mReferenceCache[location];

                         return null;
                     }).Where(a => a != null).Distinct().ToArray();

                    return ret;
                }

                return assemblies.Where(a => a.Location != string.Empty)
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
                }).Where(a => a != null).Distinct().ToArray(); //make ToArray here because of lock
            }
        }


    }
}
