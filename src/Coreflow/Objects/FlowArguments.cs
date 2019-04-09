using System;

namespace Coreflow.Objects
{
    public class FlowArguments
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public string Expression { get; set; }

        public VariableDirection Direction { get; set; }

        public FlowArguments()
        {
        }

        public FlowArguments(string pName, Type pType, VariableDirection pDirection, string pExpression = null)
        {
            Name = pName;
            Type = pType;
            Expression = pExpression;
        }
    }
}
