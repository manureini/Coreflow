using System;
using System.Collections.Generic;
using System.Text;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators.Logging
{
    public class InformationLoggerCodeCreator : AbstractLoggingCodeCreator
    {
        protected override string LogLevel => "Information";
    }
}
