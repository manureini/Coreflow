using Coreflow.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Coreflow.Storage
{
    public class DictionaryArgumentInjectionStore : IArgumentInjectionStore
    {
        private ConcurrentDictionary<string, object> mValues;

        public DictionaryArgumentInjectionStore(ConcurrentDictionary<string, object> pValues)
        {
            mValues = pValues;
        }

        public object GetArgumentValue(string pName, Type pExpectedType)
        {
            return Convert.ChangeType(mValues[pName], pExpectedType);
        }
    }
}
