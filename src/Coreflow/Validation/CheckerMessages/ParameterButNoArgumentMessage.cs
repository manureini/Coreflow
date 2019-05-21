﻿using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public class ParameterButNoArgumentMessage : IFlowValidationCodeCreatorMessage
    {
        public string Message => $"Parameter {Parameter.Name} is defined, but there is no argument with that name.";

        public FlowValidationMessageType MessageType => FlowValidationMessageType.ParameterButNoArgument;

        public bool IsFatalError => false;

        public string CodeCreatorTypeIdentifier { get; }

        public List<Guid> CodeCreatorIdentifiers { get; } = new List<Guid>();

        public CodeCreatorParameter Parameter { get; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        internal ParameterButNoArgumentMessage(string pCodeCreatorTypeIdentifier, CodeCreatorParameter pParameter, Guid pFirstCodeCreator)
        {
            CodeCreatorTypeIdentifier = pCodeCreatorTypeIdentifier;
            Parameter = pParameter;
            CodeCreatorIdentifiers.Add(pFirstCodeCreator);


        }
    }
}