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

        public FlowDefinitionFactory FlowDefinitionFactory { get; }

        public IFlowDefinitionStorage FlowDefinitionStorage { get; }

        public IFlowInstanceStorage FlowInstanceStorage { get; }

        public Coreflow(IFlowDefinitionStorage pFlowDefinitionStorage, IFlowInstanceStorage pFlowInstanceStorage)
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
        }

        public void Dispose()
        {
            FlowDefinitionStorage.Dispose();
        }
    }
}
