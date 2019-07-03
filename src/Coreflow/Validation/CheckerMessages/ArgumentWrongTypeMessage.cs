using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.CheckerMessages
{
    public class ArgumentWrongTypeMessage : IFlowValidationCodeCreatorMessage
    {
        public string Message => $"Type of argument {Parameter.Name} is wrong";

        public bool IsFatalError => true;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public CodeCreatorParameter Parameter { get; }

        public IArgument Argument { get; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        internal ArgumentWrongTypeMessage(string pCodeCreatorTypeIdentifier, CodeCreatorParameter pParameter, IArgument pArgument, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Parameter = pParameter;
            Argument = pArgument;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
