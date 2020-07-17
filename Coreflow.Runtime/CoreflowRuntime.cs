using Coreflow.Interfaces;
using Coreflow.Runtime.Storage.FlowDefinition;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Coreflow.Runtime
{
    public class CoreflowRuntime : IDisposable
    {
        public IFlowDefinitionStorage FlowDefinitionStorage { get; }

        public IFlowInstanceStorage FlowInstanceStorage { get; }

        public IArgumentInjectionStore ArgumentInjectionStore { get; }

        public RuntimeFlowManager FlowManager { get; set; } // = new FlowManager();

        public ILoggerFactory LoggerFactory { get; set; }

        public ILogger Logger { get; }

        public ILogger FlowLogger { get; }

        public string TemporaryFilesDirectory { get; protected set; }

        static CoreflowRuntime()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string toFindName = new AssemblyName(args.Name).FullName;

            if (toFindName.Contains(".resources"))
                return null;

            foreach (Assembly an in AppDomain.CurrentDomain.GetAssemblies())
                if (an.GetName().FullName == toFindName)
                    return an;

            return null;
        }

        public CoreflowRuntime(
            IFlowDefinitionStorage pFlowDefinitionStorage,
            IFlowInstanceStorage pFlowInstanceStorage,
            IArgumentInjectionStore pArgumentInjectionStore,
            ILoggerFactory pLoggerFactory = null,
            string pTemporaryFilesDirectory = null)
        {
            FlowDefinitionStorage = pFlowDefinitionStorage;
            FlowInstanceStorage = pFlowInstanceStorage;
            ArgumentInjectionStore = pArgumentInjectionStore;
            TemporaryFilesDirectory = pTemporaryFilesDirectory ?? Path.GetTempPath();

            LoggerFactory = pLoggerFactory;

            FlowManager = new RuntimeFlowManager(this);

            if (FlowDefinitionStorage is PreCompiledAssemblyFlowDefinitionStorage asmflowStorage)
            {
                FlowManager.UpdateFactories(asmflowStorage.FlowAssembly);
            }

            if (LoggerFactory == null)
            {
                LoggerFactory = new LoggerFactory();
            }

            if (!Directory.Exists(TemporaryFilesDirectory))
                Directory.CreateDirectory(TemporaryFilesDirectory);


            var oldTmpFiles = Directory.GetFiles(TemporaryFilesDirectory, "*.*", SearchOption.TopDirectoryOnly);

            /*
            foreach (string file in oldTmpFiles)
                File.Delete(file);
                */


            Logger = LoggerFactory.CreateLogger(typeof(CoreflowRuntime));
            FlowLogger = LoggerFactory.CreateLogger(typeof(ICompiledFlow));

            FlowDefinitionStorage.SetCoreflow(this);
        }

        public Guid? GetFlowIdentifier(string pFlowName)
        {
            return FlowDefinitionStorage.GetDefinitions().FirstOrDefault(d => d.Name == pFlowName)?.Identifier;
        }

        public IDictionary<string, object> RunFlow(Guid pIdentifier, IDictionary<string, object> pArguments = null)
        {
            return FlowManager.GetFactory(pIdentifier).RunInstance(pArguments);
        }

        public void Dispose()
        {
            FlowDefinitionStorage.Dispose();
        }
    }
}