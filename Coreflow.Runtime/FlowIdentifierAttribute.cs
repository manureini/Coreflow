using System;

namespace Coreflow.Objects
{
    public class FlowIdentifierAttribute : Attribute
    {
        public Guid Identifier { get; }

        public string Name { get; }

        public FlowIdentifierAttribute(string pIdentifier, string pName)
        {
            Identifier = Guid.Parse(pIdentifier);
            Name = pName;
        }
    }
}
