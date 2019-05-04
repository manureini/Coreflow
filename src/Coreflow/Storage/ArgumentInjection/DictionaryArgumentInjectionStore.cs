using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Storage
{
    public class DictionaryArgumentInjectionStore : IArgumentInjectionStore
    {
        private IDictionary<string, object> mValues;

        public DictionaryArgumentInjectionStore(IDictionary<string, object> pValues)
        {
            mValues = pValues;
        }

        public object GetArgumentValue(string pName, Type pExpectedType)
        {
            return mValues[pName];
        }
    }
}
