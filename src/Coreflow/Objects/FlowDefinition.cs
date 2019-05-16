using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;

namespace Coreflow
{
    public class FlowDefinition : IIdentifiable, IUiDesignable
    {
        public List<string> ReferencedNamespaces { get; set; }

        public List<FlowArguments> Arguments { get; set; }

        public Dictionary<Guid, Dictionary<string, object>> Metadata { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; } = "fa-cogs";

        public string Category => null;

        public ICodeCreator CodeCreator { get; set; }

        public Guid Identifier { get; set; } = Guid.NewGuid();

        private object mLocker = new object();

        // not serialized
        public Coreflow Coreflow { get; set; }

        [Obsolete("Only serializer")]
        public FlowDefinition()
        {
        }

        internal FlowDefinition(Coreflow pCoreflow)
        {
            Coreflow = pCoreflow;
        }


        public void SetMetadata(Guid pIdentifier, string pKey, object pValue)
        {
            lock (mLocker)
            {
                if (pValue == null)
                {
                    if (Metadata == null)
                        return;

                    if (!Metadata.ContainsKey(pIdentifier))
                        return;

                    var dict = Metadata[pIdentifier];
                    dict.Remove(pKey);

                    if (dict.Count <= 0)
                        Metadata.Remove(pIdentifier);

                    return;
                }

                if (Metadata == null)
                    Metadata = new Dictionary<Guid, Dictionary<string, object>>();

                if (!Metadata.ContainsKey(pIdentifier))
                    Metadata.Add(pIdentifier, new Dictionary<string, object>());

                Metadata[pIdentifier].Remove(pKey);

                Metadata[pIdentifier].Add(pKey, pValue);
            }
        }

        public object GetMetadata(Guid pIdentifier, string pKey)
        {
            lock (mLocker)
            {
                if (Metadata == null)
                    return null;

                if (!Metadata.ContainsKey(pIdentifier))
                    return null;

                if (!Metadata[pIdentifier].ContainsKey(pKey))
                    return null;

                return Metadata[pIdentifier][pKey];
            }
        }

        public FlowCode GenerateFlowCode()
        {
            return FlowBuilderHelper.GenerateFlowCode(this);
        }
    }
}
