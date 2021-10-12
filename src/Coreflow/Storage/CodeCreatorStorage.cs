using Coreflow.CodeCreators;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Objects.CodeCreatorFactory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coreflow.Storage
{
    public class CodeCreatorStorage
    {
        private readonly Dictionary<string, ICodeCreatorFactory> mCustomCodeCCreatorFactories = new Dictionary<string, ICodeCreatorFactory>();

        private readonly Dictionary<Type, ICodeCreatorFactory> mDefaultConstructorFactories = new Dictionary<Type, ICodeCreatorFactory>();

        private CoreflowService mCoreflow;

        internal CodeCreatorStorage(CoreflowService pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void AddCodeCreatorFactory(ICodeCreatorFactory pFactory)
        {
            if (mCustomCodeCCreatorFactories.ContainsKey(pFactory.Identifier))
                return;

            mCustomCodeCCreatorFactories.Add(pFactory.Identifier, pFactory);
        }

        public void AddCodeCreator(Type pCodeCreatorType)
        {
            if (mDefaultConstructorFactories.ContainsKey(pCodeCreatorType))
                return;

            if (!typeof(ICodeCreator).IsAssignableFrom(pCodeCreatorType))
                throw new ArgumentException($"type {pCodeCreatorType.FullName} does not implement {nameof(ICodeCreator)}");

            var factory = new DefaultCodeCreatorFactory(pCodeCreatorType);
            mDefaultConstructorFactories.Add(pCodeCreatorType, factory);
        }

        public void AddCodeCreatorDefaultConstructor(IEnumerable<Type> pCodeCreatorType)
        {
            foreach (var ccType in pCodeCreatorType)
            {
                AddCodeCreator(ccType);
            }
        }

        public void AddCodeActivity(IEnumerable<Type> pCodeActivityTypes)
        {
            foreach (var ccType in pCodeActivityTypes)
            {
                AddCodeActivity(ccType);
            }
        }

        public void AddCodeActivity(Type pCodeActivityType)
        {
            Type generic = typeof(CodeActivityCreator<>).MakeGenericType(pCodeActivityType);
            AddCodeCreator(generic);
        }

        public IEnumerable<ICodeCreatorFactory> GetAllFactories()
        {
            return mCustomCodeCCreatorFactories.Values.Concat(mDefaultConstructorFactories.Values);
        }

        public ICodeCreatorFactory GetFactory(string pType, string pFactoryIdentifier)
        {
            Type type = TypeHelper.SearchType(pType);

            if (mDefaultConstructorFactories.ContainsKey(type))
                return mDefaultConstructorFactories[type];

            if (pFactoryIdentifier != null && mCustomCodeCCreatorFactories.ContainsKey(pFactoryIdentifier))
                return mCustomCodeCCreatorFactories[pFactoryIdentifier];

            return new CodeCreatorMissingFactory(pType, pFactoryIdentifier);
        }

        public void RemoveFactory(string pFactoryIdentifier)
        {
            mCustomCodeCCreatorFactories.Remove(pFactoryIdentifier);
        }
    }
}
