using Coreflow.Runtime;
using System;

namespace Coreflow.Interfaces
{
    public interface ICodeActivityCreator : IIdentifiable, ICodeCreator
    {
        Type CodeActivityType { get; }
    }
}
