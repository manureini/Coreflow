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

        public ICodeCreator FindCodeCreator(Guid pGuid)
        {
            return FindCodeCreator(CodeCreator, pGuid);
        }

        private ICodeCreator FindCodeCreator(ICodeCreator pCodeCreator, Guid pGuid)
        {
            if (pCodeCreator.Identifier == pGuid)
                return pCodeCreator;

            if (pCodeCreator is ICodeCreatorContainerCreator container)
                foreach (var ccContainer in container.CodeCreators)
                {
                    foreach (var cc in ccContainer)
                    {
                        ICodeCreator found = FindCodeCreator(cc, pGuid);
                        if (found != null)
                            return found;
                    }
                }

            return null;
        }



        public ICodeCreator FindParentCodeCreatorOf(Guid pCodeCreatorIdentifier)
        {
            if (CodeCreator.Identifier == pCodeCreatorIdentifier)
                return null;

            return FindParentCodeCreatorOf(CodeCreator, pCodeCreatorIdentifier);
        }

        private ICodeCreator FindParentCodeCreatorOf(ICodeCreator pCodeCreator, Guid pCodeCreatorIdentifier)
        {
            if (pCodeCreator is ICodeCreatorContainerCreator container)
                foreach (var ccContainer in container.CodeCreators)
                {
                    foreach (var cc in ccContainer)
                    {
                        if (cc.Identifier == pCodeCreatorIdentifier)
                            return container;

                        return FindParentCodeCreatorOf(cc, pCodeCreatorIdentifier);
                    }
                }

            return null;
        }


        public FlowCode GenerateFlowCode()
        {
            return FlowBuilderHelper.GenerateFlowCode(this);
        }
    }
}
