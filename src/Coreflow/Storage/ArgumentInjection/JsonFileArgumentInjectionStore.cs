using Coreflow.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Coreflow.Storage.ArgumentInjection
{
    public class JsonFileArgumentInjectionStore : IArgumentInjectionStore
    {
        private readonly ReadOnlyDictionary<string, string> mValues;
        private readonly string mFilePath;

        public JsonFileArgumentInjectionStore(string pFilePath)
        {
            mFilePath = Path.GetFullPath(pFilePath);

            if (File.Exists(mFilePath))
            {
                string text = File.ReadAllText(mFilePath);
                mValues = new ReadOnlyDictionary<string, string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(text));
            }
            else
            {
                var example = new Dictionary<string, string>();
                example.Add("key", "value");

                string text = JsonConvert.SerializeObject(example, Formatting.Indented);
                File.WriteAllText(mFilePath, text);
            }
        }

        public object GetArgumentValue(string pName, Type pExpectedType)
        {
            if (pExpectedType != typeof(string))
                throw new NotSupportedException("Only strings are supported for " + nameof(JsonFileArgumentInjectionStore));

            if (!mValues.ContainsKey(pName))
                throw new KeyNotFoundException($"Jsonfile {mFilePath} does not contain key {pName}");

            return mValues[pName] ?? string.Empty;
        }
    }

}
