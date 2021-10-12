using System;

namespace Coreflow.Objects
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultGenericTypeAttribute : Attribute
    {
        public Type[] DefaultGenericType { get; protected set; }

        public DefaultGenericTypeAttribute(params Type[] pCode)
        {
            DefaultGenericType = pCode;
        }
    }
}
