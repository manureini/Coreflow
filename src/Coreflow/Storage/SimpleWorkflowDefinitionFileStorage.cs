using Coreflow.Helper;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Coreflow.Storage
{
    public class SimpleWorkflowDefinitionFileStorage : IWorkflowDefinitionStorage
    {
        private string mPath;
        private Coreflow mCoreflow;

        public SimpleWorkflowDefinitionFileStorage(string pPath)
        {
            mPath = pPath;
            Directory.CreateDirectory(mPath);
        }

        public void Add(WorkflowDefinition pWorkflowDefinition)
        {
            string text = WorkflowDefinitionSerializer.Serialize(pWorkflowDefinition);
            string filename = Path.Combine(mPath, pWorkflowDefinition.Identifier.ToString());
            File.WriteAllText(filename, text);
        }

        public IEnumerable<WorkflowDefinition> GetWorkflowDefinitions()
        {
            List<WorkflowDefinition> ret = new List<WorkflowDefinition>();

            foreach (string file in Directory.GetFiles(mPath))
            {
                string text = File.ReadAllText(file);
                ret.Add(WorkflowDefinitionSerializer.DeSerialize(text, mCoreflow));
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
