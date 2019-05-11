using Coreflow.Interfaces;
using System;

namespace Coreflow.Objects
{
    public class DefaultConstructorCodeCreatorFactory : ICodeCreatorFactory
    {
        public string Identifier => typeof(DefaultConstructorCodeCreatorFactory).FullName + "_" + Type.FullName;

        public Type Type { get; }

        public DefaultConstructorCodeCreatorFactory(Type pType)
        {
            Type = pType;
        }

        public ICodeCreator Create()
        {
            return Activator.CreateInstance(Type) as ICodeCreator;
        }
    }
}
