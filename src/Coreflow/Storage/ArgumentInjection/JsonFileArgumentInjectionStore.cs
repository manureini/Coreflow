using Coreflow.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Coreflow.Storage.ArgumentInjection
{
    public class JsonFileArgumentInjectionStore : IArgumentInjectionStore
    {
        private readonly ReadOnlyDictionary<string, string> mValues;

        public JsonFileArgumentInjectionStore(string pFilePath)
        {
            if (File.Exists(pFilePath))
            {
                string text = File.ReadAllText(pFilePath);
                mValues = new ReadOnlyDictionary<string, string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(text));
            }
            else
            {
                var example = new Dictionary<string, string>();
                example.TryAdd("key", "value");

                string text = JsonConvert.SerializeObject(example, Formatting.Indented);
                File.WriteAllText(pFilePath, text);
            }
        }

        public object GetArgumentValue(string pName, Type pExpectedType)
        {
            if (pExpectedType != typeof(string))
                throw new NotSupportedException("Only strings are supported for " + nameof(JsonFileArgumentInjectionStore));

            return mValues[pName] ?? string.Empty;
        }
    }

}
