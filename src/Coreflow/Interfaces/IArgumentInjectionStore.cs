using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Interfaces
{
    public interface IArgumentInjectionStore
    {
        public object GetArgumentValue(string pName, Type pExpectedType);
    }
}
