using System;

namespace Coreflow.Objects
{
    public class DefaultValueAttribute : Attribute
    {
        public string DefaultValueCode { get; protected set; }

        public DefaultValueAttribute(string pCode)
        {
            DefaultValueCode = pCode;
        }
    }
}
