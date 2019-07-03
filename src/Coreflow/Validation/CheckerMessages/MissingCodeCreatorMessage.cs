using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public class MissingCodeCreatorMessage : IFlowValidationCodeCreatorMessage
    {
        public string Message => $"CodeCreator not found!";

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public bool IsFatalError => false;

        public string Type { get; }

        public string FactoryIdentifier { get; }

        public string CodeCreatorTypeIdentifier => Type + "_" + FactoryIdentifier;

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        internal MissingCodeCreatorMessage(string pType, string pFactoryIdentifier, Guid pFirstCodeCreator)
        {
            Type = pType;
            FactoryIdentifier = pFactoryIdentifier;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
