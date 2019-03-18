using System;

namespace Coreflow.Objects
{
    public class WorkflowArguments
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public string Expression { get; set; }

        public VariableDirection Direction { get; set; }

        public WorkflowArguments()
        {
        }

        public WorkflowArguments(string pName, Type pType, VariableDirection pDirection, string pExpression = null)
        {
            Name = pName;
            Type = pType;
            Expression = pExpression;
        }
    }
}
