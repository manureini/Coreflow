using Coreflow.Helper;
using Coreflow.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Coreflow.Storage
{
    public class SimpleFlowDefinitionFileStorage : IFlowDefinitionStorage
    {
        private string mPath;
        private CoreflowRuntime mCoreflow;

        public SimpleFlowDefinitionFileStorage(string pPath)
        {
            mPath = pPath;
            Directory.CreateDirectory(mPath);
        }

        public void Add(IFlowDefinition pFlowDefinition)
        {
            string text = FlowDefinitionSerializer.Serialize(pFlowDefinition);
            string filename = Path.Combine(mPath, pFlowDefinition.Identifier.ToString());
            File.WriteAllText(filename, text);
        }

        public IEnumerable<IFlowDefinition> GetDefinitions()
        {
            List<IFlowDefinition> ret = new List<IFlowDefinition>();

            foreach (string file in Directory.GetFiles(mPath))
            {
                string text = File.ReadAllText(file);
                ret.Add(FlowDefinitionSerializer.Deserialize(text, mCoreflow));
            }

            return ret;
        }

        public IFlowDefinition Get(Guid pIdentifier)
        {
            string filename = Path.Combine(mPath, pIdentifier.ToString());

            if (!File.Exists(filename))
                return null;

            string text = File.ReadAllText(filename);
            return FlowDefinitionSerializer.Deserialize(text, mCoreflow);
        }

        public void SetCoreflow(CoreflowRuntime pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void Remove(Guid pIdentifier)
        {
            string filename = Path.Combine(mPath, pIdentifier.ToString());
            File.Delete(filename);
        }

        public void Update(IFlowDefinition pFlowDefinition)
        {
            Add(pFlowDefinition);
        }

        public void Dispose()
        {
        }
    }
}
