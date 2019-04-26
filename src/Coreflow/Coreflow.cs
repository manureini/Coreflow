using Coreflow.CodeCreators;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.CodeCreatorFactory;
using Coreflow.Storage;
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

        public FlowManager FlowManager { get; } = new FlowManager();

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

        public Coreflow(IFlowDefinitionStorage pFlowDefinitionStorage, IFlowInstanceStorage pFlowInstanceStorage, string pPluginDirectory = null)
        {
            FlowDefinitionStorage = pFlowDefinitionStorage;
            FlowInstanceStorage = pFlowInstanceStorage;

            CodeCreatorStorage = new CodeCreatorStorage(this);
            FlowDefinitionFactory = new FlowDefinitionFactory(this);


            FlowDefinitionStorage.SetCoreflow(this);

            CodeCreatorStorage.AddCodeActivity(typeof(ConsoleWriteLineActivity));
            CodeCreatorStorage.AddCodeActivity(typeof(SleepActivity));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(ForLoopCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(SequenceCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(InlineCodeCodeCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(CommentCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(AssignCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(TerminateCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(IfCreator));
            CodeCreatorStorage.AddCodeCreatorDefaultConstructor(typeof(IfElseCreator));

            if (pPluginDirectory != null)
            {
                Directory.CreateDirectory(pPluginDirectory);
                foreach (string file in Directory.GetFiles(pPluginDirectory, "*.dll", SearchOption.AllDirectories))
                {
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
            FlowManager.CompileFlows(this, FlowDefinitionStorage.GetDefinitions());
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
