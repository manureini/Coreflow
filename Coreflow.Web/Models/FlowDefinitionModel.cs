using Coreflow.Objects;
using Coreflow.Runtime;
using System;
using System.Collections.Generic;

namespace Coreflow.Web.Models
{
    public class FlowDefinitionModel : IIdentifiable
    {
        public List<string> ReferencedNamespaces { get; set; }

        public List<FlowArguments> Parameter { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public CodeCreatorModel CodeCreatorModel { get; set; }

        public Guid Identifier { get; set; }

        public string Note { get; set; }
    }
}
