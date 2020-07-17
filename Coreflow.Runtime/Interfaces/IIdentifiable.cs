using System;

namespace Coreflow.Interfaces
{
    public interface IIdentifiable
    {
        Guid Identifier { get; set; }
    }
}
