using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Web.Models
{
    public class WorkflowDefinitionModel : IIdentifiable
    {
        public List<string> ReferencedNamespaces { get; set; }

        public List<string> ReferencedAssemblies { get; set; }

        public List<WorkflowArguments> Arguments { get; set; }

        public Dictionary<string, CodeCreatorModel> CodeCreators { get; set; }

        public string Name { get; set; }

        public CodeCreatorModel CodeCreatorModel { get; set; }

        public Guid Identifier { get; set; }
    }
}
