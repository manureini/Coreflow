using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Coreflow
{
    public class FlowDefinition : IIdentifiable, IUiDesignable
    {
        public List<string> ReferencedNamespaces { get; set; }

        public List<string> ReferencedAssemblies { get; set; }

        public List<FlowArguments> Arguments { get; set; }

        public Dictionary<Guid, Dictionary<string, object>> Metadata { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; } = "fa-cogs";

        public ICodeCreator CodeCreator { get; set; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        // not serialized
        public Coreflow Coreflow { get; set; }

        [Obsolete("Only serializer")]
        public FlowDefinition()
        {
        }

        internal FlowDefinition(Coreflow pCoreflow)
        {
            Coreflow = pCoreflow;
        }

        public FlowCode GenerateFlowCode()
        {
            return FlowBuilderHelper.GenerateFlowCode(this);
        }
    }
}
