using Coreflow.Api;
using Coreflow.CodeCreators;
using Coreflow.CodeCreators.Logging;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.CodeCreatorFactory;
using Coreflow.Runtime;
using Coreflow.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Coreflow
{
    public class CoreflowService : CoreflowRuntime
    {
        public CodeCreatorStorage CodeCreatorStorage { get; }

        public FlowDefinitionFactory FlowDefinitionFactory { get; }

        public FlowCompileResult LastCompileResult { get; protected set; }

        public CoreflowApiServer ApiServer { get; protected set; }

        public void StartApiServer(object ipAddress)
        {
            throw new NotImplementedException();
        }

        public CoreflowService(
            IFlowDefinitionStorage pFlowDefinitionStorage,
            IFlowInstanceStorage pFlowInstanceStorage,
            IArgumentInjectionStore pArgumentInjectionStore,
            string pPluginDirectory = null,
            ILoggerFactory pLoggerFactory = null,
            string pTemporaryFilesDirectory = null) : base(pFlowDefinitionStorage, pFlowInstanceStorage, pArgumentInjectionStore, pLoggerFactory, pTemporaryFilesDirectory)
        {

            CodeCreatorStorage = new CodeCreatorStorage(this);
            FlowDefinitionFactory = new FlowDefinitionFactory(this);
            FlowManager = new FlowManager(this);

            CodeCreatorStorage.AddCodeActivity(typeof(ConsoleWriteLineActivity));
            CodeCreatorStorage.AddCodeActivity(typeof(SleepActivity));
            CodeCreatorStorage.AddCodeCreator(typeof(ForLoopCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(WhileCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(SequenceCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(InlineCodeCodeCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(CommentCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(DebuggerBreakCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(AssignCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(TerminateCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(IfCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(IfElseCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(PrintVariablesCreator));

            CodeCreatorStorage.AddCodeCreator(typeof(CriticalLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(DebugLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(ErrorLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(InformationLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(TraceLoggerCodeCreator));
            CodeCreatorStorage.AddCodeCreator(typeof(WarningLoggerCodeCreator));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Where(a => a != this.GetType().Assembly))
            {
                LoadPlugin(assembly);
            }

            if (pPluginDirectory != null)
            {
                Logger.LogDebug("Plugin dir: " + pPluginDirectory);
                Directory.CreateDirectory(pPluginDirectory);

                var files = Directory.GetFiles(pPluginDirectory, "*.dll", SearchOption.AllDirectories);

                List<Assembly> loadedAssemblies = new List<Assembly>();

                foreach (string file in files)
                {
                    Logger.LogDebug("Found Plugin: " + file);
                    loadedAssemblies.Add(Assembly.LoadFile(Path.GetFullPath(file)));
                }

                foreach (var asm in loadedAssemblies)
                {
                    LoadPlugin(asm);
                }
            }

            foreach (var flow in FlowDefinitionStorage.GetDefinitions())
            {
                CodeCreatorStorage.AddCodeCreatorFactory(new CallFlowCreatorFactory(this, (FlowDefinition)flow));
            }
        }

        private void LoadPlugin(Assembly asm)
        {
            var pluginType = asm.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t));

            if (pluginType.Count() > 1)
                throw new Exception($"{asm.FullName} contains multple {nameof(IPlugin)} classes");

            if (pluginType.Any())
            {
                var pluginInstance = (IPlugin)Activator.CreateInstance(pluginType.First());
                pluginInstance.OnEnable();
            }

            var types = asm.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract && !typeof(IArgument).IsAssignableFrom(t));

            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(types.Where(t => typeof(ICodeCreator).IsAssignableFrom(t)));
            CodeCreatorStorage.AddCodeActivity(types.Where(t => typeof(ICodeActivity).IsAssignableFrom(t)));
        }

        public FlowCompileResult CompileFlows(bool pDebug = true, bool pForceRecompile = false)
        {
            var result = ((FlowManager)FlowManager).CompileFlowsCreateAndLoadAssembly(FlowDefinitionStorage.GetDefinitions(), pDebug, pForceRecompile);

            if (result != null)
            {
                LastCompileResult = result;
            }

            return result;
        }

        public void StartApiServer(IPAddress pLocalIpAddress, int pPort)
        {
            ApiServer = new CoreflowApiServer(this, pLocalIpAddress, pPort);
            ApiServer.Start();
        }
    }
}
