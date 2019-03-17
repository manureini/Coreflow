using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Coreflow
{
    public class WorkflowDefinition : IIdentifiable, IUiDesignable
    {
        public List<string> ReferencedNamespaces { get; set; }

        public List<Assembly> ReferencedAssemblies { get; set; }

        public List<WorkflowArguments> Arguments { get; set; }

        public Dictionary<Guid, Dictionary<string, object>> Metadata { get; set; }

        public string Name { get; set; }

        public string Icon => "fa-cogs";

        public ICodeCreator CodeCreator { get; set; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        // not serialized
        public Coreflow Coreflow { get; internal set; }

        [Obsolete("Only serializer")]
        public WorkflowDefinition()
        {
        }

        internal WorkflowDefinition(Coreflow pCoreflow)
        {
            Coreflow = pCoreflow;
        }

        public WorkflowCode GenerateWorkflowCode()
        {
            return WorkflowBuilderHelper.GenerateWorkflowCode(this);
        }
    }
}
