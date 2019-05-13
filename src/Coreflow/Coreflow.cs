using Coreflow.CodeCreators;
using Coreflow.CodeCreators.Logging;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects.CodeCreatorFactory;
using Coreflow.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Coreflow
{
    public class Coreflow : IDisposable
    {
        public CodeCreatorStorage CodeCreatorStorage { get; }

        public FlowDefinitionFactory FlowDefinitionFactory { get; }

        public IFlowDefinitionStorage FlowDefinitionStorage { get; }

        public IFlowInstanceStorage FlowInstanceStorage { get; }

        public IArgumentInjectionStore ArgumentInjectionStore { get; }

        public FlowManager FlowManager { get; } = new FlowManager();

        public ILoggerFactory LoggerFactory { get; set; }

        public ILogger Logger { get; }

        public ILogger FlowLogger { get; }

        static Coreflow()
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

        public Coreflow(
            IFlowDefinitionStorage pFlowDefinitionStorage,
            IFlowInstanceStorage pFlowInstanceStorage,
            IArgumentInjectionStore pArgumentInjectionStore,
            string pPluginDirectory = null,
            ILoggerFactory pLoggerFactory = null)
        {
            FlowDefinitionStorage = pFlowDefinitionStorage;
            FlowInstanceStorage = pFlowInstanceStorage;
            ArgumentInjectionStore = pArgumentInjectionStore;

            CodeCreatorStorage = new CodeCreatorStorage(this);
            FlowDefinitionFactory = new FlowDefinitionFactory(this);

            LoggerFactory = pLoggerFactory;

            if (LoggerFactory == null)
            {
                LoggerFactory = new LoggerFactory();
            }

            Logger = LoggerFactory.CreateLogger(typeof(Coreflow));
            FlowLogger = LoggerFactory.CreateLogger(typeof(ICompiledFlow));


            FlowDefinitionStorage.SetCoreflow(this);

            CodeCreatorStorage.AddCodeActivity(typeof(ConsoleWriteLineActivity));
            CodeCreatorStorage.AddCodeActivity(typeof(SleepActivity));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(ForLoopCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(WhileCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(SequenceCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(InlineCodeCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(CommentCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(DebuggerBreakCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(AssignCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(TerminateCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(IfCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(IfElseCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(PrintVariablesCreator));

            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(CriticalLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(DebugLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(ErrorLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(InformationLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(TraceLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(WarningLoggerCodeCreator));

            if (pPluginDirectory != null)
            {
                Logger.LogDebug("Plugin dir: " + pPluginDirectory);
                Directory.CreateDirectory(pPluginDirectory);
                foreach (string file in Directory.GetFiles(pPluginDirectory, "*.dll", SearchOption.AllDirectories))
                {
                    Logger.LogDebug("Found Plugin: " + file);
                    Assembly asm = Assembly.LoadFile(Path.GetFullPath(file));
                    CodeCreatorStorage.AddCodeCreatorDefaultConstructor(asm.GetTypes().Where(t => typeof(ICodeCreator).IsAssignableFrom(t)));
                    CodeCreatorStorage.AddCodeActivity(asm.GetTypes().Where(t => typeof(ICodeActivity).IsAssignableFrom(t)));
                }
            }

            foreach (var flow in FlowDefinitionStorage.GetDefinitions())
            {
                CodeCreatorStorage.AddCodeCreatorFactory(new CallFlowCreatorFactory(flow));
            }
        }

        public void CompileFlows()
        {
            FlowManager.CompileFlowsCreateAndLoadAssembly(this, FlowDefinitionStorage.GetDefinitions());
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
