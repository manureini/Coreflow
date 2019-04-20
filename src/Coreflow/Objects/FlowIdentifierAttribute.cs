using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Objects
{
    public class FlowIdentifierAttribute : Attribute
    {
        public Guid Identifier { get; }

        public FlowIdentifierAttribute(string pIdentifier)
        {
            Identifier = Guid.Parse(pIdentifier);
        }
    }
}
