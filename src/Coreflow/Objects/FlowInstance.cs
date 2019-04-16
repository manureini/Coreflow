using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Objects
{
    public class FlowInstance
    {
        public Guid Identifier { get; set; }

        public Guid DefinitionIdentifier { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
