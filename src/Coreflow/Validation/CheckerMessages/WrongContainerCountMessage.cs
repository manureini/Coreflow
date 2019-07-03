using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Validation.Messages
{
    public class WrongContainerCountMessage : IFlowValidationMessage
    {
        public string Message => "The container count is inconsistent";

        public Guid Identifier { get; set; }

        public bool IsFatalError => true;

        public int CurrentCount { get; }

        public int ExpectedCount { get; }

        internal WrongContainerCountMessage(Guid pCodeCreatorIdentifier, int pCurrentCount, int pExpectedCount)
        {
            Identifier = pCodeCreatorIdentifier;
            CurrentCount = pCurrentCount;
            ExpectedCount = pExpectedCount;
        }
    }
}
