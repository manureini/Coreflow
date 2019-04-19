using Coreflow.CodeCreators;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
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

            CodeCreatorStorage.AddCodeCreator(new CodeActivityCreator<ConsoleWriteLineActivity>());
            CodeCreatorStorage.AddCodeCreator(new ForLoopCreator());
            CodeCreatorStorage.AddCodeCreator(new SequenceCreator());
            CodeCreatorStorage.AddCodeCreator(new InlineCodeCodeCreator());
            CodeCreatorStorage.AddCodeCreator(new CommentCreator());
            CodeCreatorStorage.AddCodeCreator(new AssignCreator());
            CodeCreatorStorage.AddCodeCreator(new TerminateCreator());
            CodeCreatorStorage.AddCodeCreator(new IfCreator());
            CodeCreatorStorage.AddCodeCreator(new IfElseCreator());

            if (pPluginDirectory != null)
            {
                Directory.CreateDirectory(pPluginDirectory);
                foreach (string file in Directory.GetFiles(pPluginDirectory, "*.dll", SearchOption.AllDirectories))
                {
                    Assembly asm = Assembly.LoadFile(Path.GetFullPath(file));
                    CodeCreatorStorage.AddCodeCreator(asm.GetTypes().Where(t => typeof(ICodeCreator).IsAssignableFrom(t)), false);
                    CodeCreatorStorage.AddCodeActivity(asm.GetTypes().Where(t => typeof(ICodeActivity).IsAssignableFrom(t)), false);
                }
            }
        }

        public void Dispose()
        {
            FlowDefinitionStorage.Dispose();
        }
    }
}
