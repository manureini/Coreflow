using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Storage
{
    public class CodeCreatorStorage
    {
        private readonly Dictionary<Type, ICodeCreator> mCodeCreators = new Dictionary<Type, ICodeCreator>();

        private Coreflow mCoreflow;

        internal CodeCreatorStorage(Coreflow pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void AddCodeCreator(ICodeCreator pCodeCreator, bool pAddReferencesAndNamespace = true)
        {
            Type ccType = pCodeCreator.GetType();
            mCodeCreators.Add(ccType, pCodeCreator);

            if (pAddReferencesAndNamespace)
                AddNamespaceToFactory(ccType);
        }

        public void AddCodeCreator(Type pCodeCreatorType, bool pAddReferencesAndNamespace = true)
        {
            if (!(Activator.CreateInstance(pCodeCreatorType) is ICodeCreator cc))
                throw new ArgumentException($"type {pCodeCreatorType.FullName} does not implement {nameof(ICodeCreator)}");

            AddCodeCreator(cc, pAddReferencesAndNamespace);
        }

        public void AddCodeCreator(IEnumerable<Type> pCodeCreatorType, bool pAddReferencesAndNamespace = true)
        {
            pCodeCreatorType.ForEach(t => AddCodeCreator(t, pAddReferencesAndNamespace));
        }

        public void AddCodeActivity(Type pCodeActivityType, bool pAddReferencesAndNamespace = true)
        {
            Type generic = typeof(CodeActivityCreator<>).MakeGenericType(pCodeActivityType);
            var instance = Activator.CreateInstance(generic) as ICodeActivityCreator;
            AddCodeCreator(instance, false);

            if (pAddReferencesAndNamespace)
                AddNamespaceToFactory(pCodeActivityType);
        }

        private void AddNamespaceToFactory(Type pType)
        {
            mCoreflow.FlowDefinitionFactory.AddDefaultReferencedNamespace(pType.Namespace);
        }

        public void RemoveCodeCreator(Type pType)
        {
            mCodeCreators.Remove(pType);
        }

        public Dictionary<Type, ICodeCreator> GetAllCodeCreators()
        {
            return new Dictionary<Type, ICodeCreator>(mCodeCreators);
        }
    }
}
