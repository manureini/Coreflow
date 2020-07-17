using System;

namespace Coreflow.Runtime
{
    public interface IArgumentInjectionStore
    {
        object GetArgumentValue(string pName, Type pExpectedType);
    }
}
