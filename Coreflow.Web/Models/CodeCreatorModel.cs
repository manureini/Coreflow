using System;
using System.Collections.Generic;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.Web.Models
{
    public class CodeCreatorModel : IIdentifiable
    {
        public Guid Identifier { get; set; }

        public string DisplayName { get; set; }

        public string UserDisplayName { get; set; }

        public string IconClass { get; set; }

        public string Type { get; set; }

        public int SequenceCount { get; set; }

        public List<CodeCreatorParameterModel> Parameters { get; set; }

        public List<ArgumentModel> Arguments { get; set; }

        public CodeCreatorModel Parent { get; set; }

        public Dictionary<int, List<CodeCreatorModel>> CodeCreatorModels { get; set; }
    }
}
