using System;
using System.Collections.Generic;
using Coreflow.Interfaces;

namespace Coreflow.Web.Models
{
    public class CodeCreatorModel : IIdentifiable
    {
        public Guid Identifier { get; set; }

        public string DisplayName { get; set; }

        public string UserDisplayName { get; set; }

        public string IconClass { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public string CustomFactory { get; set; }

        public int SequenceCount { get; set; }

        public List<CodeCreatorParameterModel> Parameters { get; set; }

        public List<ArgumentModel> Arguments { get; set; }

        public CodeCreatorModel Parent { get; set; }

        public Dictionary<int, List<CodeCreatorModel>> CodeCreatorModels { get; set; }

        public string UserNote { get; set; }

        public string UserColor { get; set; }
    }
}
