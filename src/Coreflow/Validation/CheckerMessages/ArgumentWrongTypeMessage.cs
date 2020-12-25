using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Validation.Messages;
using System;
using System.Collections.Generic;

namespace Coreflow.Validation.CheckerMessages
{
    public class ArgumentWrongTypeMessage : IFlowValidationCodeCreatorMessage
    {
        public string Message => $"Type of argument {Parameter.Name} is wrong.\nCurrent: {CurrentType}\nExpected: {ExpectedType?.AssemblyQualifiedName}";

        public bool IsFatalError => true;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public CodeCreatorParameter Parameter { get; }

        public IArgument Argument { get; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string CurrentType { get; }

        public Type ExpectedType { get; }

        internal ArgumentWrongTypeMessage(string pCodeCreatorTypeIdentifier, CodeCreatorParameter pParameter, IArgument pArgument, Guid pFirstCodeCreator, string pCurrentType, Type pExpectedType)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Parameter = pParameter;
            Argument = pArgument;
            CurrentType = pCurrentType;
            ExpectedType = pExpectedType;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);
        }
    }
}
