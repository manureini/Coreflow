using System;

namespace Coreflow.Objects
{
    public class WorkflowArguments
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public string DefaultExpression { get; set; }

        public WorkflowArguments()
        {
        }

        public WorkflowArguments(string pName, Type pType, string pDefaultExpression = null)
        {
            Name = pName;
            Type = pType;
            DefaultExpression = pDefaultExpression;
        }
    }
}
