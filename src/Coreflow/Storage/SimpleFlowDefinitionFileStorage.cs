using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public IEnumerable<FlowDefinition> GetFlowDefinitions()
        {
            List<FlowDefinition> ret = new List<FlowDefinition>();

            foreach (string file in Directory.GetFiles(mPath))
            {
                string text = File.ReadAllText(file);
                ret.Add(FlowDefinitionSerializer.DeSerialize(text, mCoreflow));
            }

            return ret;
        }

        public void Dispose()
        {
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
    }
}
