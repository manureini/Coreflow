using System;

namespace Coreflow.Objects
{
    public class FlowArgument
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public string Expression { get; set; }

        public VariableDirection Direction { get; set; }

        public FlowArgument()
        {
        }

        public FlowArgument(string pName, Type pType, VariableDirection pDirection, string pExpression = null)
        {
            Name = pName;
            Type = pType;
            Direction = pDirection;
            Expression = pExpression;
        }
    }
}
