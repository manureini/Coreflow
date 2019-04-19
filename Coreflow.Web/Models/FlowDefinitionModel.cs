using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow.Web.Models
{
    public class FlowDefinitionModel : IIdentifiable
    {
        public List<string> ReferencedNamespaces { get; set; }

        public List<string> ReferencedAssemblies { get; set; }

        public List<FlowArguments> Arguments { get; set; }

        public List<CodeCreatorModel> CodeCreators { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public CodeCreatorModel CodeCreatorModel { get; set; }

        public Guid Identifier { get; set; }
    }
}
