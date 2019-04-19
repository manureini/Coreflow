using System;
using System.Collections.Generic;

namespace Coreflow.Interfaces
{
    public interface ICompiledFlow
    {
        Guid InstanceId { get; }

        void SetArguments(IDictionary<string, object> pArguments);

        IDictionary<string, object> GetArguments();

        void Run();
    }
}