using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using System;

namespace Coreflow.Objects.CodeCreatorFactory
{
    public class CodeCreatorMissingFactory : ICodeCreatorFactory
    {
        public string Identifier => typeof(CodeCreatorMissingFactory).FullName + "_" + Type + "_" + FactoryIdentifier + "_" + Guid.NewGuid();

        private string Type { get; }

        private string FactoryIdentifier { get; }

        public CodeCreatorMissingFactory(string pType, string pFactoryIdentifier)
        {
            Type = pType;
            FactoryIdentifier = pFactoryIdentifier;
        }

        public ICodeCreator Create()
        {
            return new MissingCodeCreatorCreator(Type, Identifier);
        }
    }
}
