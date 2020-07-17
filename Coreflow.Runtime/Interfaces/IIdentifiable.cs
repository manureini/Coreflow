using System;

namespace Coreflow.Runtime
{
    public interface IIdentifiable
    {
        Guid Identifier { get; set; }
    }
}
