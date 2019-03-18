using Coreflow.CodeCreators;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow
{
    public class Coreflow : IDisposable
    {
        public CodeCreatorStorage CodeCreatorStorage { get; }

        public WorkflowDefinitionFactory WorkflowDefinitionFactory { get; }

        public IWorkflowDefinitionStorage WorkflowDefinitionStorage { get; }

        public Coreflow(IWorkflowDefinitionStorage pWorkflowDefinitionStorage)
        {
            WorkflowDefinitionStorage = pWorkflowDefinitionStorage;
            CodeCreatorStorage = new CodeCreatorStorage(this);
            WorkflowDefinitionFactory = new WorkflowDefinitionFactory(this);

            CodeCreatorStorage.AddCodeCreator(new CodeActivityCreator<ConsoleWriteLineActivity>());
            CodeCreatorStorage.AddCodeCreator(new ForLoopCreator());
            CodeCreatorStorage.AddCodeCreator(new SequenceCreator());
            CodeCreatorStorage.AddCodeCreator(new InlineCodeCodeCreator());
            CodeCreatorStorage.AddCodeCreator(new CommentCreator());
            CodeCreatorStorage.AddCodeCreator(new AssignCreator());
            CodeCreatorStorage.AddCodeCreator(new TerminateCreator());
            CodeCreatorStorage.AddCodeCreator(new IfCreator());
        }

        public void Dispose()
        {
            WorkflowDefinitionStorage.Dispose();
        }
    }
}
