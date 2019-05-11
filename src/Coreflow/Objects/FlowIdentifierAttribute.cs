using System;

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
