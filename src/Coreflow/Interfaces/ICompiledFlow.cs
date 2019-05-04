using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Coreflow.Interfaces
{
    public interface ICompiledFlow
    {
        Guid InstanceId { get; set; }

        Coreflow CoreflowInstace { get; set; }

        IArgumentInjectionStore ArgumentInjectionStore { get; set; }

        ILogger Logger { get; set; }

        void SetArguments(IDictionary<string, object> pArguments);

        IDictionary<string, object> GetArguments();

        void Run();
    }
}