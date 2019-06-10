using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Coreflow.Storage
{
    public class SimpleFlowDefinitionFileStorage : IFlowDefinitionStorage
    {
        private string mPath;
        private Coreflow mCoreflow;

        public SimpleFlowDefinitionFileStorage(string pPath)
        {
            mPath = pPath;
            Directory.CreateDirectory(mPath);
        }

        public void Add(FlowDefinition pFlowDefinition)
        {
            string text = FlowDefinitionSerializer.Serialize(pFlowDefinition);
            string filename = Path.Combine(mPath, pFlowDefinition.Identifier.ToString());
            File.WriteAllText(filename, text);
        }

        public IEnumerable<FlowDefinition> GetDefinitions()
        {
            List<FlowDefinition> ret = new List<FlowDefinition>();

            foreach (string file in Directory.GetFiles(mPath))
            {
                string text = File.ReadAllText(file);
                ret.Add(FlowDefinitionSerializer.Deserialize(text, mCoreflow));
            }

            return ret;
        }

        public FlowDefinition Get(Guid pIdentifier)
        {
            string filename = Path.Combine(mPath, pIdentifier.ToString());
            string text = File.ReadAllText(filename);
            return FlowDefinitionSerializer.Deserialize(text, mCoreflow);
        }

        public void SetCoreflow(Coreflow pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void Remove(Guid pIdentifier)
        {
            string filename = Path.Combine(mPath, pIdentifier.ToString());
            File.Delete(filename);
        }

        public void Dispose()
        {
        }
    }
}
