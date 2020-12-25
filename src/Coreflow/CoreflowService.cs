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

                var files = Directory.GetFiles(pPluginDirectory, "*.dll", SearchOption.AllDirectories);

                List<Assembly> loadedAssemblies = new List<Assembly>();

                foreach (string file in files)
                {
                    Logger.LogDebug("Found Plugin: " + file);
                    loadedAssemblies.Add(Assembly.LoadFile(Path.GetFullPath(file)));
                }

                foreach (var asm in loadedAssemblies)
                {
                    var pluginType = asm.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t));
                    if (pluginType.Count() > 1)
                        throw new Exception($"{asm.FullName} contains multple {nameof(IPlugin)} classes");

                    if (pluginType.Any())
                    {
                        var pluginInstance = (IPlugin)Activator.CreateInstance(pluginType.First());
                        pluginInstance.OnEnable();
                    }

                    CodeCreatorStorage.AddCodeCreatorDefaultConstructor(asm.GetTypes().Where(t => typeof(ICodeCreator).IsAssignableFrom(t)));
                    CodeCreatorStorage.AddCodeActivity(asm.GetTypes().Where(t => typeof(ICodeActivity).IsAssignableFrom(t)));
                }
            }

            foreach (var flow in FlowDefinitionStorage.GetDefinitions())
            {
                CodeCreatorStorage.AddCodeCreatorFactory(new CallFlowCreatorFactory(this, (FlowDefinition)flow));
            }
        }

        public void CompileFlows()
        {
            var result = ((FlowManager)FlowManager).CompileFlowsCreateAndLoadAssembly(FlowDefinitionStorage.GetDefinitions());

            if (result != null)
            {
                LastCompileResult = result;
            }
        }

        public void StartApiServer(IPAddress pLocalIpAddress, int pPort)
        {
            ApiServer = new CoreflowApiServer(this, pLocalIpAddress, pPort);
            ApiServer.Start();
        }
    }
}
