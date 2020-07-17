using System;
using System.Collections.Generic;

namespace Coreflow.Validation
{
    public class CorrectorData
    {
        public string Type { get; set; }

        public object Data { get; set; }

        public List<Guid> CodeCreators { get; set; }
    }
}
