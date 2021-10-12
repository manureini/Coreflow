using System;

namespace Coreflow.Objects
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DefaultValueAttribute : Attribute
    {
        public string DefaultValueCode { get; protected set; }

        public DefaultValueAttribute(string pCode)
        {
            DefaultValueCode = pCode;
        }
    }
}
