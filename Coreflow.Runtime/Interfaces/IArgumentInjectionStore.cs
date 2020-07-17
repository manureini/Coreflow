using System;

namespace Coreflow.Interfaces
{
    public interface IArgumentInjectionStore
    {
        object GetArgumentValue(string pName, Type pExpectedType);
    }
}
