using System;

namespace Coreflow.Interfaces
{
    public interface IArgumentInjectionStore
    {
        public object GetArgumentValue(string pName, Type pExpectedType);
    }
}
